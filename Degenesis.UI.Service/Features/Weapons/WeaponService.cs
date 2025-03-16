using Degenesis.Shared.DTOs.Weapons;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Weapons;
public class WeaponService
{
    private readonly HttpClient _httpClient;

    public WeaponService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<WeaponDto>> GetWeaponsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<WeaponDto>>("/weapons") ?? new();
    }

    public async Task<WeaponDto> CreateWeaponAsync(WeaponCreateDto weapon)
    {
        var response = await _httpClient.PostAsJsonAsync("/weapons", weapon);
        return await response.Content.ReadFromJsonAsync<WeaponDto>();
    }

    public async Task UpdateWeaponAsync(WeaponDto weapon)
    {
        await _httpClient.PutAsJsonAsync($"/weapons", weapon);
    }

    public async Task DeleteWeaponAsync(Guid weaponId)
    {
        await _httpClient.DeleteAsync($"/weapons/{weaponId}");
    }
}
