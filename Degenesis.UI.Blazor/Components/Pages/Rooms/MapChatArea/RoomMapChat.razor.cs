using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.Display;
using Degenesis.Shared.DTOs.NPCs;
using Degenesis.Shared.DTOs.Rooms;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms.MapChatArea;

public partial class RoomMapChat : IAsyncDisposable
{
    [Parameter] public Guid IdRoom { get; set; }
    [Parameter] public CharacterDisplayDto? Character { get; set; }
    [Parameter] public bool IsGM { get; set; }
    [Parameter] public List<CharacterDisplayForGMListDto> GmCharacters { get; set; } = [];
    [Parameter] public List<NPCDto> GmNpcs { get; set; } = [];
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] private RoomConnectionService RoomConnection { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private RoomDisplayDto? room;
    private HubConnection? hubConnection;
    private string currentMessage = "";
    private string? userName;
    private static readonly string[] DiceFaces = ["⚀", "⚁", "⚂", "⚃", "⚄", "⚅"];
    private bool isRollsDrawerOpen;
    private bool isMembersDrawerOpen;
    private bool isMapsDrawerOpen;
    private RoomMapDto? SelectedMap;
    private const double MinMapZoom = 0.5;
    private const double MaxMapZoom = 3;
    private const double MapZoomStep = 0.25;
    private double _mapZoom = 1;
    private double _mapOffsetX;
    private double _mapOffsetY;
    private double _lastPointerX;
    private double _lastPointerY;
    private bool isMapDragging;
    private bool _isSavingMapTokens;
    private ElementReference _mapSvg;
    private IJSObjectReference? _mapCanvasModule;
    private RoomMapToken? _pendingMapToken;
    private RoomMapToken? _draggingToken;
    private RoomMapToken? _contextMenuToken;
    private double _contextMenuClientX;
    private double _contextMenuClientY;
    private readonly Dictionary<Guid, List<RoomMapToken>> _tokensByMap = [];
    private List<RoomMapToken> CurrentMapTokens => SelectedMap is not null && _tokensByMap.TryGetValue(SelectedMap.Id, out var tokens) ? tokens : [];
    private double MapTokenRadius => SelectedMap is null ? 20 : Math.Max(12, Math.Min(SelectedMap.Width, SelectedMap.Height) * 0.025);
    private double TokenLabelSize => MapTokenRadius * 0.65;
    private string MapTransform => FormattableString.Invariant($"translate({_mapOffsetX}px, {_mapOffsetY}px) scale({_mapZoom})");
    private static string SvgNumber(double value) => FormattableString.Invariant($"{value}");
    private string ContextMenuStyle => $"position: fixed; left: {SvgNumber(_contextMenuClientX)}px; top: {SvgNumber(_contextMenuClientY)}px; z-index: 20; min-width: 120px;";
    private string MapImageApiRoot => Client?.BaseAddress is null
        ? string.Empty
        : new Uri(Client.BaseAddress, "..").GetLeftPart(UriPartial.Authority);

    private List<RoomChatMessageDto> Messages = [];

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        AuthenticationStateProvider.AuthenticationStateChanged += async _ =>
        {
            await RefreshUserNameAsync();
        };

        await RefreshUserNameAsync();

        var roomResult = await Client!.GetFromJsonAsync<Result<RoomDisplayDto>>($"/rooms/{IdRoom}") ?? new Result<RoomDisplayDto> { IsError = true, Error = "Unknown error", Value = null };
        if (roomResult.IsError)
            Snackbar.Add(roomResult.Error ?? "Unknown error", Severity.Error);

        room = roomResult.Value;

        if (room is null)
        {
            Navigation.NavigateTo("/rooms");
            return;
        }

        await LoadSelectedMapAsync();

        // Create a connection with the chat hub
        hubConnection = await RoomConnection.GetOrCreateAsync(IdRoom);

        // Detach if a previous connection existed
        hubConnection.Remove("ReceiveMessage");
        hubConnection.Remove("ReceiveHistory");
        hubConnection.Remove("ReceiveRollResult");

        // When hub send message to user
        hubConnection.On<string, string>("ReceiveMessage", (sender, message) =>
        {
            // Insert at position 0 : displayed at the bottom thanks to flex-column-reverse
            Messages.Insert(0, new RoomChatMessageDto { Sender = sender, Text = message, SentAtUtc = DateTime.UtcNow });
            InvokeAsync(StateHasChanged);
        });

        // When hub send messages history to user
        hubConnection.On<List<RoomChatMessageDto>>("ReceiveHistory", history =>
        {
            // History arrives chronological (oldest first) → reverse it
            // so that the newest is first (bottom of the visual column)
            Messages = history;
            Messages.Reverse();
            InvokeAsync(StateHasChanged);
        });

        // When hub send a dice roll to display
        hubConnection.On<DiceRollResultDto>("ReceiveRollResult", result =>
        {
            Messages.Insert(0, new RoomChatMessageDto
            {
                Sender = result.Roller,
                Text = result.RollDescription,
                SentAtUtc = DateTime.UtcNow,
                RollResult = result
            });
            InvokeAsync(StateHasChanged);
        });

        await hubConnection.SendAsync("GetHistory", IdRoom.ToString());
    }

    private async Task RefreshUserNameAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            Navigation.NavigateTo("/login");
            return;
        }

        userName = user.FindFirst("unique_name")?.Value;
        StateHasChanged();
    }

    private void ToggleMapDrawer() => isRollsDrawerOpen = !isRollsDrawerOpen;
    private void ToggleMembersDrawer() => isMembersDrawerOpen = !isMembersDrawerOpen;
    private void ToggleMapsDrawer() => isMapsDrawerOpen = !isMapsDrawerOpen;

    private async Task SelectMap(RoomMapDto map)
    {
        SelectedMap = map;
        _mapZoom = 1;
        _mapOffsetX = 0;
        _mapOffsetY = 0;
        isMapDragging = false;
        _draggingToken = null;
        isMapsDrawerOpen = false;

        if (IsGM)
        {
            var response = await Client!.PutAsync($"/room-maps/room/{IdRoom}/select/{map.Id}", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Result<object>>();
                Snackbar.Add(error?.Error ?? "Unable to save the selected map", Severity.Error);
            }
            else
            {
                room!.SelectedMapId = map.Id;
            }
        }

        await LoadMapTokensAsync();
    }

    private async Task LoadSelectedMapAsync()
    {
        if (room?.SelectedMapId is null)
            return;

        var result = await Client!.GetFromJsonAsync<Result<List<RoomMapDto>>>($"/room-maps/room/{IdRoom}");
        if (result is null || result.IsError)
            return;

        var selectedMap = result.Value?.FirstOrDefault(map => map.Id == room.SelectedMapId);
        if (selectedMap is null)
            return;

        SelectedMap = selectedMap;
        await LoadMapTokensAsync();
    }

    private async Task StartMapDrag(PointerEventArgs args)
    {
        _contextMenuToken = null;

        if (args.Button != 0)
            return;

        isMapDragging = true;
        _lastPointerX = args.ClientX;
        _lastPointerY = args.ClientY;
        await CaptureMapPointerAsync(args.PointerId);
    }

    private async Task StartTokenDrag(RoomMapToken token, PointerEventArgs args)
    {
        if (args.Button != 0)
            return;

        _draggingToken = token;
        await CaptureMapPointerAsync(args.PointerId);
    }

    private async Task DragMap(PointerEventArgs args)
    {
        var draggingToken = _draggingToken;
        if (draggingToken is not null)
        {
            var selectedMap = SelectedMap;
            if (selectedMap is null)
                return;

            var mapTokenRadius = Math.Max(12, Math.Min(selectedMap.Width, selectedMap.Height) * 0.025);
            var point = await GetMapPointAsync(args.ClientX, args.ClientY);
            if (!ReferenceEquals(_draggingToken, draggingToken) || !ReferenceEquals(SelectedMap, selectedMap))
                return;

            draggingToken.X = Math.Clamp(point.X, mapTokenRadius, Math.Max(mapTokenRadius, selectedMap.Width - mapTokenRadius));
            draggingToken.Y = Math.Clamp(point.Y, mapTokenRadius, Math.Max(mapTokenRadius, selectedMap.Height - mapTokenRadius));
            return;
        }

        if (!isMapDragging)
            return;

        _mapOffsetX += args.ClientX - _lastPointerX;
        _mapOffsetY += args.ClientY - _lastPointerY;
        _lastPointerX = args.ClientX;
        _lastPointerY = args.ClientY;
    }

    private void EndMapDrag(PointerEventArgs args)
    {
        isMapDragging = false;
        _draggingToken = null;
    }

    private async Task LoadMapTokensAsync()
    {
        if (SelectedMap is null)
            return;

        var result = await Client!.GetFromJsonAsync<Result<List<RoomMapTokenDto>>>($"/room-maps/{SelectedMap.Id}/tokens");
        var tokens = (result?.Value ?? [])
            .Select(savedToken => new RoomMapToken
            {
                MemberId = savedToken.MemberId,
                IsNpc = savedToken.IsNpc,
                Name = savedToken.Name,
                AvatarName = savedToken.AvatarName,
                X = savedToken.X,
                Y = savedToken.Y
            })
            .ToList();

        if (result is not null && result.IsError)
            Snackbar.Add(result.Error ?? "Unable to load map tokens", Severity.Error);

        _tokensByMap[SelectedMap.Id] = tokens;
    }

    private void OpenTokenContextMenu(RoomMapToken token, MouseEventArgs args)
    {
        _contextMenuToken = token;
        _contextMenuClientX = args.ClientX;
        _contextMenuClientY = args.ClientY;
    }

    private async Task DeleteContextMenuToken()
    {
        if (_contextMenuToken is null || SelectedMap is null)
            return;

        var tokenToDelete = _contextMenuToken;
        var remainingTokens = CurrentMapTokens
            .Where(token => !ReferenceEquals(token, tokenToDelete))
            .Select(token => new RoomMapTokenDto
            {
                MemberId = token.MemberId,
                IsNpc = token.IsNpc,
                Name = token.Name,
                AvatarName = token.AvatarName,
                X = token.X,
                Y = token.Y
            })
            .ToList();

        var response = await Client!.PutAsJsonAsync($"/room-maps/{SelectedMap.Id}/tokens", remainingTokens);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unable to delete map token", Severity.Error);
            return;
        }

        CurrentMapTokens.Remove(tokenToDelete);
        _contextMenuToken = null;
        Snackbar.Add("Map token deleted", Severity.Success);
    }

    private async Task SaveMapTokens()
    {
        if (SelectedMap is null)
            return;

        _isSavingMapTokens = true;
        var tokens = CurrentMapTokens.Select(token => new RoomMapTokenDto
        {
            MemberId = token.MemberId,
            IsNpc = token.IsNpc,
            Name = token.Name,
            AvatarName = token.AvatarName,
            X = token.X,
            Y = token.Y
        }).ToList();

        var response = await Client!.PutAsJsonAsync($"/room-maps/{SelectedMap.Id}/tokens", tokens);
        if (!response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result<object>>();
            Snackbar.Add(result?.Error ?? "Unable to save map tokens", Severity.Error);
        }
        else
        {
            Snackbar.Add("Map tokens saved", Severity.Success);
        }

        _isSavingMapTokens = false;
    }

    private void SetPendingMapToken(RoomMapToken token) => _pendingMapToken = token;

    private void IgnoreMapDragOver(DragEventArgs args) { }

    private async Task DropToken(DragEventArgs args)
    {
        var pendingToken = _pendingMapToken;
        var selectedMap = SelectedMap;
        if (pendingToken is null || selectedMap is null)
            return;

        var mapTokenRadius = Math.Max(12, Math.Min(selectedMap.Width, selectedMap.Height) * 0.025);
        var point = await GetMapPointAsync(args.ClientX, args.ClientY);
        var token = new RoomMapToken
        {
            MemberId = pendingToken.MemberId,
            IsNpc = pendingToken.IsNpc,
            Name = pendingToken.Name,
            AvatarName = pendingToken.AvatarName,
            X = Math.Clamp(point.X, mapTokenRadius, Math.Max(mapTokenRadius, selectedMap.Width - mapTokenRadius)),
            Y = Math.Clamp(point.Y, mapTokenRadius, Math.Max(mapTokenRadius, selectedMap.Height - mapTokenRadius))
        };

        if (!_tokensByMap.TryGetValue(selectedMap.Id, out var tokens))
        {
            tokens = [];
            _tokensByMap.Add(selectedMap.Id, tokens);
        }

        var existingToken = tokens.FirstOrDefault(item => item.Key == token.Key);
        if (existingToken is null)
            tokens.Add(token);
        else
        {
            existingToken.X = token.X;
            existingToken.Y = token.Y;
        }

        if (ReferenceEquals(_pendingMapToken, pendingToken))
            _pendingMapToken = null;
    }

    private async Task CaptureMapPointerAsync(long pointerId)
    {
        try
        {
            await (await GetMapCanvasModuleAsync()).InvokeVoidAsync("capturePointer", _mapSvg, pointerId);
        }
        catch(JSDisconnectedException)
        {
            // If the JS runtime is disconnected, we can't capture the pointer, so we just ignore it.
        }
    }

    private async Task<SvgPoint> GetMapPointAsync(double clientX, double clientY)
    {
        try
        {
            return await (await GetMapCanvasModuleAsync()).InvokeAsync<SvgPoint>("getSvgPoint", _mapSvg, clientX, clientY);
        }
        catch(JSDisconnectedException)
        {
            // If the JS runtime is disconnected, we can't get the SVG point, so we return a default point.
            return new SvgPoint { X = 0, Y = 0 };
        }
    }

    private async Task<IJSObjectReference> GetMapCanvasModuleAsync()
    {
        try
        {
            return _mapCanvasModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/map-canvas.js");
        }
        catch (JSDisconnectedException)
        {
            // If the JS runtime is disconnected, we can't import the module, so we throw an exception.
            throw new InvalidOperationException("JS runtime is disconnected.");
        }
    }

    private string GetTokenAvatarUrl(RoomMapToken token)
    {
        var avatarType = token.IsNpc ? "npc-avatars" : "character-avatars";
        return $"{MapImageApiRoot}/{avatarType}/{Uri.EscapeDataString(token.AvatarName!)}";
    }

    private static string GetInitials(string name)
        => string.IsNullOrWhiteSpace(name) ? "?" : name[..1].ToUpperInvariant();

    private sealed class SvgPoint
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    private void ZoomIn() => _mapZoom = Math.Min(_mapZoom + MapZoomStep, MaxMapZoom);
    private void ZoomOut() => _mapZoom = Math.Max(_mapZoom - MapZoomStep, MinMapZoom);

    private string GetSelectedMapImageUrl()
        => $"{MapImageApiRoot}/map-images/{Uri.EscapeDataString(SelectedMap!.FileName)}";

    // When user send message to hub
    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(currentMessage))
        {
            await hubConnection!.SendAsync("SendMessage", IdRoom.ToString(), userName, currentMessage);
            currentMessage = "";
        }
    }

    private static string GetDieFace(int value) => value is >= 1 and <= 6 ? DiceFaces[value - 1] : value.ToString();

    // Have to do this GetDiceStyle else Blazor aint happy in the dice display
    private static string GetDieStyle(int value)
    {
        var color = value switch
        {
            1 => "darkred",
            2 or 3 => "red",
            4 or 5 => "green",
            6 => "goldenrod",
            _ => ""
        };
        return string.IsNullOrEmpty(color) ? "" : $"color: {color};";
    }

    public async ValueTask DisposeAsync()
    {
        // Never dispose hubConnection here: it belongs to RoomConnectionService.
        // Just detach our handlers so a dead component stops receiving events.
        hubConnection?.Remove("ReceiveMessage");
        hubConnection?.Remove("ReceiveHistory");
        hubConnection?.Remove("ReceiveRollResult");
        if (_mapCanvasModule is not null)
            await _mapCanvasModule.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}