using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;

public class BackgroundService
{
    private readonly HttpClient _httpClient;

    public BackgroundService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BackgroundDto>> GetBackgroundsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BackgroundDto>>("/backgrounds") ?? new();
    }

    public async Task<BackgroundDto> CreateBackgroundAsync(BackgroundCreateDto background)
    {
        var response = await _httpClient.PostAsJsonAsync("/backgrounds", background);
        return await response.Content.ReadFromJsonAsync<BackgroundDto>();
    }

    public async Task UpdateBackgroundAsync(BackgroundDto background)
    {
        await _httpClient.PutAsJsonAsync($"/backgrounds", background);
    }

    public async Task DeleteBackgroundAsync(Guid backgroundId)
    {
        await _httpClient.DeleteAsync($"/backgrounds/{backgroundId}");
    }
}
