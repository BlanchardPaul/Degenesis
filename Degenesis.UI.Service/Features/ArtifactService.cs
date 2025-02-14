using Degenesis.Shared.DTOs.Artifacts;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features;

public class ArtifactService
{
    private readonly HttpClient _httpClient;

    public ArtifactService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ArtifactDto>> GetArtifactsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<ArtifactDto>>("/artifacts") ?? new();
    }

    public async Task<ArtifactDto> CreateArtifactAsync(ArtifactCreateDto artifact)
    {
        var response = await _httpClient.PostAsJsonAsync("/artifacts", artifact);
        return await response.Content.ReadFromJsonAsync<ArtifactDto>();
    }

    public async Task UpdateArtifactAsync(ArtifactDto artifact)
    {
        await _httpClient.PutAsJsonAsync($"/artifacts", artifact);
    }

    public async Task DeleteArtifactAsync(Guid artifactId)
    {
        await _httpClient.DeleteAsync($"/artifacts/{artifactId}");
    }
}
