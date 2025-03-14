using Degenesis.Shared.DTOs.Protections;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Protections;
public class ProtectionQualityService
{
    private readonly HttpClient _httpClient;

    public ProtectionQualityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProtectionQualityDto>> GetProtectionQualitysAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProtectionQualityDto>>("/protection-qualities") ?? new();
    }

    public async Task<ProtectionQualityDto> CreateProtectionQualityAsync(ProtectionQualityCreateDto protectionQuality)
    {
        var response = await _httpClient.PostAsJsonAsync("/protection-qualities", protectionQuality);
        return await response.Content.ReadFromJsonAsync<ProtectionQualityDto>();
    }

    public async Task UpdateProtectionQualityAsync(ProtectionQualityDto protectionQuality)
    {
        await _httpClient.PutAsJsonAsync($"/protection-qualities", protectionQuality);
    }

    public async Task DeleteProtectionQualityAsync(Guid protectionQualityId)
    {
        await _httpClient.DeleteAsync($"/protection-qualities/{protectionQualityId}");
    }
}
