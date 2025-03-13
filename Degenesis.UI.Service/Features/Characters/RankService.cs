using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class RankService
{

    private readonly HttpClient _httpClient;

    public RankService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RankDto>> GetRanksAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<RankDto>>("/ranks") ?? new();
    }

    public async Task<RankDto> CreateRankAsync(RankCreateDto rank)
    {
        var response = await _httpClient.PostAsJsonAsync("/ranks", rank);
        return await response.Content.ReadFromJsonAsync<RankDto>();
    }

    public async Task UpdateRankAsync(RankDto rank)
    {
        await _httpClient.PutAsJsonAsync($"/ranks", rank);
    }

    public async Task DeleteRankAsync(Guid rankId)
    {
        await _httpClient.DeleteAsync($"/ranks/{rankId}");
    }

}
