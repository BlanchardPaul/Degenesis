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
using MudBlazor.Utilities;

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
    private bool isGridDrawerOpen;
    private bool _isGridVisible = true;
    private string _mapInteractionMode = "Interaction";
    private SvgPoint? _measurementStart;
    private SvgPoint? _measurementEnd;
    private MudColor _drawingColor = new("#f44336");
    private int _drawingStrokeWidth = 4;
    private RoomMapDrawingDto? _activeDrawing;
    private bool IsMeasurementMode => _mapInteractionMode == "Measurement";
    private bool IsDrawingMode => _mapInteractionMode == "Drawing";
    private int MeasurementDistance => _measurementStart is null || _measurementEnd is null
        ? 0
        : (int)Math.Round(Math.Sqrt(Math.Pow(_measurementEnd.X - _measurementStart.X, 2) + Math.Pow(_measurementEnd.Y - _measurementStart.Y, 2)) / GridCellSize, MidpointRounding.AwayFromZero);
    private bool _isSavingGrid;
    private int _gridCellSize = 100;
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
    private double _draggingTokenStartX;
    private double _draggingTokenStartY;
    private ElementReference _mapSvg;
    private IJSObjectReference? _mapCanvasModule;
    private RoomMapToken? _pendingMapToken;
    private RoomMapToken? _draggingToken;
    private RoomMapToken? _contextMenuToken;
    private RoomMapDrawingDto? _contextMenuDrawing;
    private double _contextMenuClientX;
    private double _contextMenuClientY;
    private readonly Dictionary<Guid, List<RoomMapToken>> _tokensByMap = [];
    private readonly Dictionary<Guid, List<RoomMapDrawingDto>> _drawingsByMap = [];
    private List<RoomMapToken> CurrentMapTokens => SelectedMap is not null && _tokensByMap.TryGetValue(SelectedMap.Id, out var tokens) ? tokens : [];
    private List<RoomMapDrawingDto> CurrentMapDrawings => SelectedMap is not null && _drawingsByMap.TryGetValue(SelectedMap.Id, out var drawings) ? drawings : [];
    private int MaximumGridCellSize => SelectedMap is null ? 300 : Math.Min(300, Math.Max(SelectedMap.Width, SelectedMap.Height));
    private int MinimumGridCellSize => SelectedMap is null ? 1 : Math.Min(MaximumGridCellSize, Math.Max(1, (int)Math.Ceiling(Math.Max(SelectedMap.Width, SelectedMap.Height) / 200d)));
    private double GridCellSize => Math.Clamp(_gridCellSize, MinimumGridCellSize, MaximumGridCellSize);
    private int GridColumns => SelectedMap is null ? 0 : (int)Math.Ceiling(SelectedMap.Width / GridCellSize);
    private int GridRows => SelectedMap is null ? 0 : (int)Math.Ceiling(SelectedMap.Height / GridCellSize);
    private double TokenLabelSize => GridCellSize * 0.65;
    private string MapTransform => FormattableString.Invariant($"translate({_mapOffsetX}px, {_mapOffsetY}px) scale({_mapZoom})");
    private static string SvgNumber(double value) => FormattableString.Invariant($"{value}");
    private string ContextMenuStyle => $"position: fixed; left: {SvgNumber(_contextMenuClientX)}px; top: {SvgNumber(_contextMenuClientY)}px; z-index: 20; min-width: 120px;";
    private static string GetDrawingPoints(IEnumerable<RoomMapPointDto> points)
        => string.Join(" ", points.Select(point => $"{SvgNumber(point.X)},{SvgNumber(point.Y)}"));
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
        hubConnection.Remove("ReceiveRoomMapSelected");
        hubConnection.Remove("ReceiveGridConfigurationUpdated");
        hubConnection.Remove("ReceiveMapStateUpdated");

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

        hubConnection.On<Guid>("ReceiveRoomMapSelected", mapId => InvokeAsync(() => LoadAndSelectMapAsync(mapId)));

        hubConnection.On<Guid, int, bool>("ReceiveGridConfigurationUpdated", (mapId, gridCellSize, isGridVisible) =>
            InvokeAsync(() =>
            {
                if (SelectedMap?.Id == mapId)
                {
                    SelectedMap.GridCellSize = gridCellSize;
                    SelectedMap.IsGridVisible = isGridVisible;
                    _gridCellSize = gridCellSize;
                    _isGridVisible = isGridVisible;
                }

                StateHasChanged();
            }));

        hubConnection.On<Guid, RoomMapStateDto>("ReceiveMapStateUpdated", (mapId, state) =>
            InvokeAsync(() => ApplyMapState(mapId, state)));

        hubConnection.On<Guid, RoomMapDrawingDto>("ReceiveMapDrawingAdded", (mapId, drawing) =>
            InvokeAsync(() =>
            {
                if (!_drawingsByMap.TryGetValue(mapId, out var drawings))
                {
                    drawings = [];
                    _drawingsByMap.Add(mapId, drawings);
                }

                if (drawings.All(existing => existing.Id != drawing.Id))
                    drawings.Add(drawing);
                StateHasChanged();
            }));

        hubConnection.On<Guid, Guid>("ReceiveMapDrawingDeleted", (mapId, drawingId) =>
            InvokeAsync(() =>
            {
                if (_drawingsByMap.TryGetValue(mapId, out var drawings))
                    drawings.RemoveAll(drawing => drawing.Id == drawingId);
                StateHasChanged();
            }));

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

    private void ToggleSavedRollsDrawer() => isRollsDrawerOpen = !isRollsDrawerOpen;
    private void ToggleMembersDrawer() => isMembersDrawerOpen = !isMembersDrawerOpen;
    private void ToggleMapsDrawer() => isMapsDrawerOpen = !isMapsDrawerOpen;
    private void ToggleGridDrawer() => isGridDrawerOpen = !isGridDrawerOpen;

    private async Task SaveGridConfigurationAsync()
    {
        if (SelectedMap is null || _isSavingGrid)
            return;

        if (_gridCellSize < MinimumGridCellSize || _gridCellSize > MaximumGridCellSize)
        {
            Snackbar.Add($"Grid cell size must be between {MinimumGridCellSize} and {MaximumGridCellSize} pixels", Severity.Warning);
            return;
        }

        _isSavingGrid = true;
        try
        {
            var response = await Client!.PutAsync($"/room-maps/{SelectedMap.Id}/grid?gridCellSize={_gridCellSize}&isGridVisible={_isGridVisible}", null);
            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result<object>>();
                Snackbar.Add(result?.Error ?? "Unable to save grid size", Severity.Error);
                return;
            }

            SelectedMap.GridCellSize = _gridCellSize;
            SelectedMap.IsGridVisible = _isGridVisible;
            Snackbar.Add("Grid configuration saved", Severity.Success);
        }
        finally
        {
            _isSavingGrid = false;
        }
    }

    private Task SelectMap(RoomMapDto map)
        => SelectMapAsync(map, persistSelection: true);

    private async Task SelectMapAsync(RoomMapDto map, bool persistSelection)
    {
        if (IsGM && persistSelection)
        {
            var response = await Client!.PutAsync($"/room-maps/room/{IdRoom}/select/{map.Id}", null);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Result<object>>();
                Snackbar.Add(error?.Error ?? "Unable to save the selected map", Severity.Error);
                return;
            }

            room!.SelectedMapId = map.Id;
            if (SelectedMap?.Id == map.Id)
                return;
        }

        SelectedMap = map;
        _gridCellSize = Math.Clamp(map.GridCellSize, MinimumGridCellSize, MaximumGridCellSize);
        _isGridVisible = map.IsGridVisible;
        _mapZoom = 1;
        _mapOffsetX = 0;
        _mapOffsetY = 0;
        isMapDragging = false;
        _draggingToken = null;
        _measurementStart = null;
        _measurementEnd = null;
        _activeDrawing = null;
        _contextMenuToken = null;
        _contextMenuDrawing = null;
        isMapsDrawerOpen = false;

        await LoadMapTokensAsync();
        await LoadMapDrawingsAsync();
    }

    private async Task LoadAndSelectMapAsync(Guid mapId)
    {
        if (SelectedMap?.Id == mapId)
            return;

        var result = await Client!.GetFromJsonAsync<Result<List<RoomMapDto>>>($"/room-maps/room/{IdRoom}");
        if (result is null || result.IsError)
        {
            Snackbar.Add(result?.Error ?? "Unable to load selected map", Severity.Error);
            return;
        }

        var map = result.Value?.FirstOrDefault(candidate => candidate.Id == mapId);
        if (map is null)
        {
            Snackbar.Add("Selected map was not found", Severity.Error);
            return;
        }

        if (room is not null)
            room.SelectedMapId = mapId;
        await SelectMapAsync(map, persistSelection: false);
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
        _gridCellSize = Math.Clamp(selectedMap.GridCellSize, MinimumGridCellSize, MaximumGridCellSize);
        _isGridVisible = selectedMap.IsGridVisible;
        await LoadMapTokensAsync();
        await LoadMapDrawingsAsync();
    }

    private async Task StartMapDrag(PointerEventArgs args)
    {
        _contextMenuToken = null;
        _contextMenuDrawing = null;

        if (_isSavingMapTokens || args.Button != 0)
            return;

        if (IsMeasurementMode)
        {
            var map = SelectedMap;
            if (map is null)
                return;

            var point = await GetMapPointAsync(args.ClientX, args.ClientY);
            if (!ReferenceEquals(SelectedMap, map))
                return;

            _measurementStart = point;
            _measurementEnd = point;
            await CaptureMapPointerAsync(args.PointerId);
            return;
        }

        if (IsDrawingMode)
        {
            var map = SelectedMap;
            if (map is null)
                return;

            var point = await GetMapPointAsync(args.ClientX, args.ClientY);
            if (!ReferenceEquals(SelectedMap, map))
                return;

            if (!_drawingsByMap.TryGetValue(map.Id, out var drawings))
            {
                drawings = [];
                _drawingsByMap.Add(map.Id, drawings);
            }

            _activeDrawing = new RoomMapDrawingDto
            {
                Id = Guid.NewGuid(),
                Color = _drawingColor.ToString(MudColorOutputFormats.HexA),
                StrokeWidth = _drawingStrokeWidth
            };
            _activeDrawing.Points.Add(GetDrawingPoint(point, map));
            drawings.Add(_activeDrawing);
            await CaptureMapPointerAsync(args.PointerId);
            return;
        }

        isMapDragging = true;
        _lastPointerX = args.ClientX;
        _lastPointerY = args.ClientY;
        await CaptureMapPointerAsync(args.PointerId);
    }

    private async Task StartTokenDrag(RoomMapToken token, PointerEventArgs args)
    {
        if (_isSavingMapTokens || IsMeasurementMode || IsDrawingMode || args.Button != 0)
            return;

        _draggingToken = token;
        _draggingTokenStartX = token.X;
        _draggingTokenStartY = token.Y;
        await CaptureMapPointerAsync(args.PointerId);
    }

    private async Task DragMap(PointerEventArgs args)
    {
        var activeDrawing = _activeDrawing;
        if (IsDrawingMode && activeDrawing is not null)
        {
            var selectedMap = SelectedMap;
            if (selectedMap is null)
                return;

            var point = await GetMapPointAsync(args.ClientX, args.ClientY);
            if (ReferenceEquals(SelectedMap, selectedMap) && ReferenceEquals(_activeDrawing, activeDrawing))
                activeDrawing.Points.Add(GetDrawingPoint(point, selectedMap));
            return;
        }

        if (IsMeasurementMode && _measurementStart is not null)
        {
            var selectedMap = SelectedMap;
            if (selectedMap is null)
                return;

            var point = await GetMapPointAsync(args.ClientX, args.ClientY);
            if (ReferenceEquals(SelectedMap, selectedMap) && _measurementStart is not null)
                _measurementEnd = point;
            return;
        }

        var draggingToken = _draggingToken;
        if (draggingToken is not null)
        {
            var selectedMap = SelectedMap;
            if (selectedMap is null)
                return;

            var point = await GetMapPointAsync(args.ClientX, args.ClientY);
            if (!ReferenceEquals(_draggingToken, draggingToken) || !ReferenceEquals(SelectedMap, selectedMap))
                return;

            var tokenPosition = GetTokenPosition(point, selectedMap);
            draggingToken.X = tokenPosition.X;
            draggingToken.Y = tokenPosition.Y;
            return;
        }

        if (!isMapDragging)
            return;

        _mapOffsetX += args.ClientX - _lastPointerX;
        _mapOffsetY += args.ClientY - _lastPointerY;
        _lastPointerX = args.ClientX;
        _lastPointerY = args.ClientY;
    }

    private async Task EndMapDrag(PointerEventArgs args)
    {
        var selectedMap = SelectedMap;
        var completedDrawing = _activeDrawing;
        var movedToken = _draggingToken;
        var tokenMoved = movedToken is not null &&
                         (movedToken.X != _draggingTokenStartX || movedToken.Y != _draggingTokenStartY);

        isMapDragging = false;
        _draggingToken = null;
        _measurementStart = null;
        _measurementEnd = null;
        _activeDrawing = null;

        if (selectedMap is null || _isSavingMapTokens)
            return;

        if (completedDrawing is not null)
        {
            _isSavingMapTokens = true;
            try
            {
                var saved = await SaveMapDrawingAsync(selectedMap, completedDrawing);
                if (!saved && _drawingsByMap.TryGetValue(selectedMap.Id, out var drawings))
                    drawings.RemoveAll(drawing => drawing.Id == completedDrawing.Id);
            }
            finally
            {
                _isSavingMapTokens = false;
            }
        }
        else if (tokenMoved && movedToken is not null)
        {
            _isSavingMapTokens = true;
            try
            {
                if (!await SaveMapTokensAsync(selectedMap, CurrentMapTokens))
                {
                    movedToken.X = _draggingTokenStartX;
                    movedToken.Y = _draggingTokenStartY;
                }
            }
            finally
            {
                _isSavingMapTokens = false;
            }
        }
    }

    private async Task LoadMapTokensAsync()
    {
        var selectedMap = SelectedMap;
        if (selectedMap is null)
            return;

        var result = await Client!.GetFromJsonAsync<Result<List<RoomMapTokenDto>>>($"/room-maps/{selectedMap.Id}/tokens");
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

        if (ReferenceEquals(SelectedMap, selectedMap))
            _tokensByMap[selectedMap.Id] = tokens;
    }

    private void ApplyMapState(Guid mapId, RoomMapStateDto state)
    {
        _tokensByMap[mapId] = (state.Tokens ?? [])
            .Select(token => new RoomMapToken
            {
                MemberId = token.MemberId,
                IsNpc = token.IsNpc,
                Name = token.Name,
                AvatarName = token.AvatarName,
                X = token.X,
                Y = token.Y
            })
            .ToList();
        _drawingsByMap[mapId] = state.Drawings ?? [];
        StateHasChanged();
    }

    private async Task LoadMapDrawingsAsync()
    {
        var selectedMap = SelectedMap;
        if (selectedMap is null)
            return;

        var result = await Client!.GetFromJsonAsync<Result<List<RoomMapDrawingDto>>>($"/room-maps/{selectedMap.Id}/drawings");
        if (result is null || result.IsError)
        {
            Snackbar.Add(result?.Error ?? "Unable to load map drawings", Severity.Error);
            return;
        }

        if (ReferenceEquals(SelectedMap, selectedMap))
            _drawingsByMap[selectedMap.Id] = result.Value ?? [];
    }

    private void OpenTokenContextMenu(RoomMapToken token, MouseEventArgs args)
    {
        _contextMenuToken = token;
        _contextMenuDrawing = null;
        _contextMenuClientX = args.ClientX;
        _contextMenuClientY = args.ClientY;
    }

    private void OpenDrawingContextMenu(RoomMapDrawingDto drawing, MouseEventArgs args)
    {
        _contextMenuToken = null;
        _contextMenuDrawing = drawing;
        _contextMenuClientX = args.ClientX;
        _contextMenuClientY = args.ClientY;
    }

    private Task DeleteContextMenuItem()
        => _contextMenuToken is not null ? DeleteContextMenuToken() : DeleteContextMenuDrawing();

    private async Task DeleteContextMenuToken()
    {
        if (_contextMenuToken is null || SelectedMap is null)
            return;

        var selectedMap = SelectedMap;
        var tokenToDelete = _contextMenuToken;
        var remainingTokens = CurrentMapTokens
            .Where(token => !ReferenceEquals(token, tokenToDelete))
            .ToList();

        _isSavingMapTokens = true;
        try
        {
            if (!await SaveMapTokensAsync(selectedMap, remainingTokens))
                return;

            CurrentMapTokens.Remove(tokenToDelete);
            _contextMenuToken = null;
            _contextMenuDrawing = null;
            Snackbar.Add("Map token deleted", Severity.Success);
        }
        finally
        {
            _isSavingMapTokens = false;
        }
    }

    private async Task DeleteContextMenuDrawing()
    {
        var drawingToDelete = _contextMenuDrawing;
        var selectedMap = SelectedMap;
        if (!IsGM || drawingToDelete is null || selectedMap is null || _isSavingMapTokens)
            return;

        _isSavingMapTokens = true;
        try
        {
            var response = await Client!.DeleteAsync($"/room-maps/{selectedMap.Id}/drawings/{drawingToDelete.Id}");
            if (!response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Result<object>>();
                Snackbar.Add(result?.Error ?? "Unable to delete map drawing", Severity.Error);
                return;
            }

            CurrentMapDrawings.RemoveAll(drawing => drawing.Id == drawingToDelete.Id);
            _contextMenuDrawing = null;
            Snackbar.Add("Map drawing deleted", Severity.Success);
        }
        finally
        {
            _isSavingMapTokens = false;
        }
    }

    private async Task<bool> SaveMapDrawingAsync(RoomMapDto map, RoomMapDrawingDto drawing)
    {
        var response = await Client!.PostAsJsonAsync($"/room-maps/{map.Id}/drawings", drawing);
        if (response.IsSuccessStatusCode)
            return true;

        var result = await response.Content.ReadFromJsonAsync<Result<object>>();
        Snackbar.Add(result?.Error ?? "Unable to save map drawing", Severity.Error);
        return false;
    }

    private async Task<bool> SaveMapTokensAsync(RoomMapDto map, List<RoomMapToken> tokens)
    {
        var tokenDtos = tokens.Select(token => new RoomMapTokenDto
        {
            MemberId = token.MemberId,
            IsNpc = token.IsNpc,
            Name = token.Name,
            AvatarName = token.AvatarName,
            X = token.X,
            Y = token.Y
        }).ToList();

        var response = await Client!.PutAsJsonAsync($"/room-maps/{map.Id}/tokens", tokenDtos);
        if (response.IsSuccessStatusCode)
            return true;

        var result = await response.Content.ReadFromJsonAsync<Result<object>>();
        Snackbar.Add(result?.Error ?? "Unable to save map tokens", Severity.Error);
        return false;
    }

    private void SetPendingMapToken(RoomMapToken token) => _pendingMapToken = token;

    private void IgnoreMapDragOver(DragEventArgs args) { }

    private static RoomMapPointDto GetDrawingPoint(SvgPoint point, RoomMapDto map)
        => new()
        {
            X = Math.Clamp(point.X, 0, map.Width),
            Y = Math.Clamp(point.Y, 0, map.Height)
        };

    private SvgPoint GetTokenPosition(SvgPoint point, RoomMapDto map)
    {
        var halfTokenSize = GridCellSize / 2;
        var minimumX = Math.Min(halfTokenSize, map.Width / 2d);
        var maximumX = Math.Max(minimumX, map.Width - halfTokenSize);
        var minimumY = Math.Min(halfTokenSize, map.Height / 2d);
        var maximumY = Math.Max(minimumY, map.Height - halfTokenSize);

        if (_isGridVisible)
        {
            point.X = Math.Round((point.X - halfTokenSize) / GridCellSize, MidpointRounding.AwayFromZero) * GridCellSize + halfTokenSize;
            point.Y = Math.Round((point.Y - halfTokenSize) / GridCellSize, MidpointRounding.AwayFromZero) * GridCellSize + halfTokenSize;
        }

        return new SvgPoint
        {
            X = Math.Clamp(point.X, minimumX, maximumX),
            Y = Math.Clamp(point.Y, minimumY, maximumY)
        };
    }

    private async Task DropToken(DragEventArgs args)
    {
        var pendingToken = _pendingMapToken;
        var selectedMap = SelectedMap;
        if (!IsGM || _isSavingMapTokens || pendingToken is null || selectedMap is null)
            return;

        var point = await GetMapPointAsync(args.ClientX, args.ClientY);
        var tokenPosition = GetTokenPosition(point, selectedMap);
        var token = new RoomMapToken
        {
            MemberId = pendingToken.MemberId,
            IsNpc = pendingToken.IsNpc,
            Name = pendingToken.Name,
            AvatarName = pendingToken.AvatarName,
            X = tokenPosition.X,
            Y = tokenPosition.Y
        };

        if (!_tokensByMap.TryGetValue(selectedMap.Id, out var tokens))
        {
            tokens = [];
            _tokensByMap.Add(selectedMap.Id, tokens);
        }

        var existingToken = tokens.FirstOrDefault(item => item.Key == token.Key);
        var previousX = existingToken?.X;
        var previousY = existingToken?.Y;
        if (existingToken is null)
            tokens.Add(token);
        else
        {
            existingToken.X = token.X;
            existingToken.Y = token.Y;
        }

        _isSavingMapTokens = true;
        try
        {
            if (!await SaveMapTokensAsync(selectedMap, tokens))
            {
                if (existingToken is null)
                    tokens.Remove(token);
                else
                {
                    existingToken.X = previousX!.Value;
                    existingToken.Y = previousY!.Value;
                }
                return;
            }
        }
        finally
        {
            _isSavingMapTokens = false;
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

    private void ZoomMapWithWheel(WheelEventArgs args)
    {
        if (args.DeltaY < 0)
            _mapZoom = Math.Min(_mapZoom + MapZoomStep, MaxMapZoom);
        else if (args.DeltaY > 0)
            _mapZoom = Math.Max(_mapZoom - MapZoomStep, MinMapZoom);
    }

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
        hubConnection?.Remove("ReceiveRoomMapSelected");
        hubConnection?.Remove("ReceiveGridConfigurationUpdated");
        hubConnection?.Remove("ReceiveMapStateUpdated");
        hubConnection?.Remove("ReceiveMapDrawingAdded");
        hubConnection?.Remove("ReceiveMapDrawingDeleted");
        if (_mapCanvasModule is not null)
            await _mapCanvasModule.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}