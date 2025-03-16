using Degenesis.Shared.DTOs.Weapons;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Weapons;
public class WeaponTypeService
{
    private readonly HttpClient _httpClient;

    public WeaponTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<WeaponTypeDto>> GetWeaponTypesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<WeaponTypeDto>>("/weapon-types") ?? new();
    }

    public async Task<WeaponTypeDto> CreateWeaponTypeAsync(WeaponTypeCreateDto weaponType)
    {
        var response = await _httpClient.PostAsJsonAsync("/weapon-types", weaponType);
        return await response.Content.ReadFromJsonAsync<WeaponTypeDto>();
    }

    public async Task UpdateWeaponTypeAsync(WeaponTypeDto weaponType)
    {
        await _httpClient.PutAsJsonAsync($"/weapon-types", weaponType);
    }

    public async Task DeleteWeaponTypeAsync(Guid weaponTypeId)
    {
        await _httpClient.DeleteAsync($"/weapon-types/{weaponTypeId}");
    }

}

