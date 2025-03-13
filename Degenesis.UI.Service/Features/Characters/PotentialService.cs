using Degenesis.Shared.DTOs.Characters;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Characters;
public class PotentialService
{
    private readonly HttpClient _httpClient;

    public PotentialService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PotentialDto>> GetPotentialsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<PotentialDto>>("/potentials") ?? new();
    }

    public async Task<PotentialDto> CreatePotentialAsync(PotentialCreateDto potential)
    {
        var response = await _httpClient.PostAsJsonAsync("/potentials", potential);
        return await response.Content.ReadFromJsonAsync<PotentialDto>();
    }

    public async Task UpdatePotentialAsync(PotentialDto potential)
    {
        await _httpClient.PutAsJsonAsync($"/potentials", potential);
    }

    public async Task DeletePotentialAsync(Guid potentialId)
    {
        await _httpClient.DeleteAsync($"/potentials/{potentialId}");
    }

}
