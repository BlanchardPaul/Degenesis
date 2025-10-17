using Degenesis.Shared.DTOs.Rooms;
using Degenesis.UI.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace Degenesis.UI.Blazor.Components.Pages.Rooms;

public partial class RoomMapChat : ComponentBase, IAsyncDisposable
{
    [Parameter] public Guid RoomId { get; set; }

    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;
    [Inject] private AuthenticatedHttpClientService AuthenticatedHttpClientService { get; set; } = default!;

    private RoomDisplayDto? room;
    private HubConnection? hubConnection;
    private string currentMessage = "";
    private string? userName;
    private HttpClient _client = new();

    private class ChatMessage
    {
        public string Sender { get; set; } = "";
        public string Text { get; set; } = "";
    }

    private List<ChatMessage> Messages = [];

    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            Navigation.NavigateTo("/login");
            return;
        }

        userName = user.FindFirst("unique_name")?.Value;

        _client = await AuthenticatedHttpClientService.GetClientAsync();
        room = await _client.GetFromJsonAsync<RoomDisplayDto>($"/rooms/{RoomId}");

        if (room is null || string.IsNullOrEmpty(userName) || !room.Players.Contains(userName) && room.GMName != userName)
        {
            Navigation.NavigateTo("/rooms");
            return;
        }

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

        hubConnection.On<string, string>("ReceiveMessage", (sender, message) =>
        {
            Messages.Add(new ChatMessage { Sender = sender, Text = message });
            InvokeAsync(StateHasChanged);
        });

        await hubConnection.StartAsync();
        await hubConnection.SendAsync("JoinRoom", RoomId.ToString());
    }

    private async Task SendMessage()
    {
        if (!string.IsNullOrWhiteSpace(currentMessage))
        {
            await hubConnection!.SendAsync("SendMessage", RoomId.ToString(), userName, currentMessage);
            currentMessage = "";
            StateHasChanged();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.SendAsync("LeaveRoom", RoomId.ToString());
            await hubConnection.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}