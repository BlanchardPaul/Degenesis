using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class SkillService
{
    private readonly HttpClient _httpClient;

    public SkillService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SkillDto>> GetSkillsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<SkillDto>>("/skills") ?? new();
    }

    public async Task<SkillDto> CreateSkillAsync(SkillCreateDto skill)
    {
        var response = await _httpClient.PostAsJsonAsync("/skills", skill);
        return await response.Content.ReadFromJsonAsync<SkillDto>();
    }

    public async Task UpdateSkillAsync(SkillDto skill)
    {
        await _httpClient.PutAsJsonAsync($"/skills", skill);
    }

    public async Task DeleteSkillAsync(Guid skillId)
    {
        await _httpClient.DeleteAsync($"/skills/{skillId}");
    }
}
