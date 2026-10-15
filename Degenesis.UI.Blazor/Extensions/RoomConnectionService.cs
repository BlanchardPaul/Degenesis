using Microsoft.AspNetCore.SignalR.Client;

namespace Degenesis.UI.Blazor.Extensions;

public class RoomConnectionService : IAsyncDisposable
{
    private readonly AuthenticatedHttpClientService _authHttpClient;
    private HubConnection? _connection;
    private Guid _currentRoomId;
    private readonly List<Action<HubConnection>> _connectionReadyCallbacks = [];

    public HubConnection? Connection => _connection;
    public Guid CurrentRoomId => _currentRoomId;

    public RoomConnectionService(AuthenticatedHttpClientService authHttpClient)
    {
        _authHttpClient = authHttpClient;
    }

    public bool IsForRoom(Guid roomId) => _connection is not null && _currentRoomId == roomId;

    public async Task<HubConnection> GetOrCreateAsync(Guid roomId, string userName)
    {
        if (_connection is not null)
        {
            if (_currentRoomId == roomId)
                return _connection;

            await _connection.SendAsync("LeaveRoom", _currentRoomId.ToString());
            await _connection.DisposeAsync();
            _connection = null;
        }

        _currentRoomId = roomId;

        _connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7274/roomchathub", options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    var token = await _authHttpClient.GetTokenAsync();
                    return token ?? string.Empty;
                };
            })
            .WithAutomaticReconnect()
            .Build();

        await _connection.StartAsync();
        await _connection.SendAsync("JoinRoom", roomId.ToString());

        foreach (var callback in _connectionReadyCallbacks)
            callback(_connection);

        return _connection;
    }

    public void OnConnectionReady(Action<HubConnection> callback)
    {
        if (_connection is not null)
            callback(_connection);
        else
            _connectionReadyCallbacks.Add(callback);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            if (_connection.State == HubConnectionState.Connected)
                await _connection.SendAsync("LeaveRoom", _currentRoomId.ToString());
            await _connection.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}