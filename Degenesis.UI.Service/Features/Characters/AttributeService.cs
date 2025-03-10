using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class AttributeService
{
    private readonly HttpClient _httpClient;

    public AttributeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AttributeDto>> GetAttributesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<AttributeDto>>("/attributes") ?? new();
    }

    public async Task<AttributeDto> CreateAttributeAsync(AttributeCreateDto attribute)
    {
        var response = await _httpClient.PostAsJsonAsync("/attributes", attribute);
        return await response.Content.ReadFromJsonAsync<AttributeDto>();
    }

    public async Task UpdateAttributeAsync(AttributeDto attribute)
    {
        await _httpClient.PutAsJsonAsync($"/attributes", attribute);
    }

    public async Task DeleteAttributeAsync(Guid attributeId)
    {
        await _httpClient.DeleteAsync($"/attributes/{attributeId}");
    }
}
