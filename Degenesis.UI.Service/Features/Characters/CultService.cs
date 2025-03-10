using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class CultService
{
    private readonly HttpClient _httpClient;

    public CultService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CultDto>> GetCultsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CultDto>>("/cults") ?? new();
    }

    public async Task<CultDto> CreateCultAsync(CultCreateDto cult)
    {
        var response = await _httpClient.PostAsJsonAsync("/cults", cult);
        return await response.Content.ReadFromJsonAsync<CultDto>();
    }

    public async Task UpdateCultAsync(CultDto cult)
    {
        await _httpClient.PutAsJsonAsync($"/cults", cult);
    }

    public async Task DeleteCultAsync(Guid cultId)
    {
        await _httpClient.DeleteAsync($"/cults/{cultId}");
    }
}
