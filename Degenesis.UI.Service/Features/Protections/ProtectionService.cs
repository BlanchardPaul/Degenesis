using Degenesis.Shared.DTOs.Protections;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Protections;
public class ProtectionService
{
    private readonly HttpClient _httpClient;

    public ProtectionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ProtectionDto>> GetProtectionsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ProtectionDto>>("/protections") ?? new();
    }

    public async Task<ProtectionDto> CreateProtectionAsync(ProtectionCreateDto protection)
    {
        var response = await _httpClient.PostAsJsonAsync("/protections", protection);
        return await response.Content.ReadFromJsonAsync<ProtectionDto>();
    }

    public async Task UpdateProtectionAsync(ProtectionDto protection)
    {
        await _httpClient.PutAsJsonAsync($"/protections", protection);
    }

    public async Task DeleteProtectionAsync(Guid protectionId)
    {
        await _httpClient.DeleteAsync($"/protections/{protectionId}");
    }
}
