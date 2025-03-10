using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class ConceptService
{
    private readonly HttpClient _httpClient;

    public ConceptService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ConceptDto>> GetConceptsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ConceptDto>>("/concepts") ?? new();
    }

    public async Task<ConceptDto> CreateConceptAsync(ConceptCreateDto concept)
    {
        var response = await _httpClient.PostAsJsonAsync("/concepts", concept);
        return await response.Content.ReadFromJsonAsync<ConceptDto>();
    }

    public async Task UpdateConceptAsync(ConceptDto concept)
    {
        await _httpClient.PutAsJsonAsync($"/concepts", concept);
    }

    public async Task DeleteConceptAsync(Guid conceptId)
    {
        await _httpClient.DeleteAsync($"/concepts/{conceptId}");
    }
}
