using Degenesis.Shared.DTOs.Weapons;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Weapons;
public class WeaponQualityService
{
    private readonly HttpClient _httpClient;

    public WeaponQualityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<WeaponQualityDto>> GetWeaponQualitiesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<WeaponQualityDto>>("/weapon-qualities") ?? new();
    }

    public async Task<WeaponQualityDto> CreateWeaponQualityAsync(WeaponQualityCreateDto weaponQuality)
    {
        var response = await _httpClient.PostAsJsonAsync("/weapon-qualities", weaponQuality);
        return await response.Content.ReadFromJsonAsync<WeaponQualityDto>();
    }

    public async Task UpdateWeaponQualityAsync(WeaponQualityDto weaponQuality)
    {
        await _httpClient.PutAsJsonAsync($"/weapon-qualities", weaponQuality);
    }

    public async Task DeleteWeaponQualityAsync(Guid weaponQualityId)
    {
        await _httpClient.DeleteAsync($"/weapon-qualities/{weaponQualityId}");
    }
}
