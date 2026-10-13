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
    [Inject] private AuthenticatedHttpClientService AuthenticatedHttpClientService { get; set; } = default!;

    private RoomDisplayDto? room;
    private HubConnection? hubConnection;
    private string currentMessage = "";
    private string? userName;

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
        hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7274/roomchathub", options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    var token = await AuthenticatedHttpClientService.GetTokenAsync();
                    return token ?? string.Empty;
                };
            })
            .WithAutomaticReconnect()
            .Build();

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

        await hubConnection.StartAsync();
        await hubConnection.SendAsync("JoinRoom", IdRoom.ToString());
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

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("LeaveRoom", IdRoom.ToString());
            await hubConnection.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}