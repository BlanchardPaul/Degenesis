using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Rooms;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomMapChat : IAsyncDisposable
{
    [Parameter] public Guid IdRoom { get; set; }

    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] private RoomConnectionService RoomConnection { get; set; } = default!;

    private RoomDisplayDto? room;
    private HubConnection? hubConnection;
    private string currentMessage = "";
    private string? userName;
    private static readonly string[] DiceFaces = ["⚀", "⚁", "⚂", "⚃", "⚄", "⚅"];

    private List<RoomChatMessageDto> Messages = [];

    protected override async Task OnAuthenticatedInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            Navigation.NavigateTo("/login");
            return;
        }

        userName = user.FindFirst("unique_name")?.Value;

        var roomResult = await Client!.GetFromJsonAsync<Result<RoomDisplayDto>>($"/rooms/{IdRoom}") ?? new Result<RoomDisplayDto> { IsError = true, Error = "Unknown error", Value = null };
        if (roomResult.IsError)
            Snackbar.Add(roomResult.Error ?? "Unknown error", Severity.Error);

        room = roomResult.Value;

        if (room is null || string.IsNullOrEmpty(userName) || !room.Players.Contains(userName) && room.GMName != userName)
        {
            Navigation.NavigateTo("/rooms");
            return;
        }

        // Create a connection with the chat hub
        hubConnection = await RoomConnection.GetOrCreateAsync(IdRoom, userName!);

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

    // When user send message to hub
    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(currentMessage))
        {
            await hubConnection!.SendAsync("SendMessage", IdRoom.ToString(), userName, currentMessage);
            currentMessage = "";
            StateHasChanged();
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

    public ValueTask DisposeAsync()
    {
        // Never dispose hubConnection here: it belongs to RoomConnectionService.
        // Just detach our handlers so a dead component stops receiving events.
        hubConnection?.Remove("ReceiveMessage");
        hubConnection?.Remove("ReceiveHistory");
        hubConnection?.Remove("ReceiveRollResult");
        return ValueTask.CompletedTask;
    }
}