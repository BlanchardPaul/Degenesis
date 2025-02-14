using Degenesis.Shared.DTOs.Burns;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features;

public class BurnService
{
    private readonly HttpClient _httpClient;

    public BurnService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<BurnDto>> GetBurnsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<BurnDto>>("/burns") ?? new();
    }

    public async Task<BurnDto> CreateBurnAsync(BurnCreateDto burn)
    {
        var response = await _httpClient.PostAsJsonAsync("/burns", burn);
        return await response.Content.ReadFromJsonAsync<BurnDto>();
    }

    public async Task UpdateBurnAsync(BurnDto burn)
    {
        await _httpClient.PutAsJsonAsync($"/burns", burn);
    }

    public async Task DeleteBurnAsync(Guid burnId)
    {
        await _httpClient.DeleteAsync($"/burns/{burnId}");
    }
}
