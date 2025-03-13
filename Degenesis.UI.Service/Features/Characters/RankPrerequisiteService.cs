using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class RankPrerequisiteService
{

    private readonly HttpClient _httpClient;

    public RankPrerequisiteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<RankPrerequisiteDto>> GetRankPrerequisitesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<RankPrerequisiteDto>>("/rank-prerequisites") ?? new();
    }

    public async Task<RankPrerequisiteDto> CreateRankPrerequisiteAsync(RankPrerequisiteCreateDto rankPrerequisite)
    {
        var response = await _httpClient.PostAsJsonAsync("/rank-prerequisites", rankPrerequisite);
        return await response.Content.ReadFromJsonAsync<RankPrerequisiteDto>();
    }

    public async Task UpdateRankPrerequisiteAsync(RankPrerequisiteDto rankPrerequisite)
    {
        await _httpClient.PutAsJsonAsync($"/rank-prerequisites", rankPrerequisite);
    }

    public async Task DeleteRankPrerequisiteAsync(Guid rankPrerequisiteId)
    {
        await _httpClient.DeleteAsync($"/rank-prerequisites/{rankPrerequisiteId}");
    }

}
