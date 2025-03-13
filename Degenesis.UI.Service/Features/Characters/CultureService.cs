using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class CultureService
{
    private readonly HttpClient _httpClient;

    public CultureService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CultureDto>> GetCulturesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<CultureDto>>("/cultures") ?? new();
    }

    public async Task<CultureDto> CreateCultureAsync(CultureCreateDto culture)
    {
        var response = await _httpClient.PostAsJsonAsync("/cultures", culture);
        return await response.Content.ReadFromJsonAsync<CultureDto>();
    }

    public async Task UpdateCultureAsync(CultureDto culture)
    {
        await _httpClient.PutAsJsonAsync($"/cultures", culture);
    }

    public async Task DeleteCultureAsync(Guid cultureId)
    {
        await _httpClient.DeleteAsync($"/cultures/{cultureId}");
    }
}
