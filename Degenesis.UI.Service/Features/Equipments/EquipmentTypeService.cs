using Degenesis.Shared.DTOs.Equipments;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Equipments;
public class EquipmentTypeService
{
    private readonly HttpClient _httpClient;

    public EquipmentTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EquipmentTypeDto>> GetEquipmentTypesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<EquipmentTypeDto>>("/equipment-types") ?? new();
    }

    public async Task<EquipmentTypeDto> CreateEquipmentTypeAsync(EquipmentTypeCreateDto equipmentType)
    {
        var response = await _httpClient.PostAsJsonAsync("/equipment-types", equipmentType);
        return await response.Content.ReadFromJsonAsync<EquipmentTypeDto>();
    }

    public async Task UpdateEquipmentTypeAsync(EquipmentTypeDto equipmentType)
    {
        await _httpClient.PutAsJsonAsync($"/equipment-types", equipmentType);
    }

    public async Task DeleteEquipmentTypeAsync(Guid equipmentTypeId)
    {
        await _httpClient.DeleteAsync($"/equipment-types/{equipmentTypeId}");
    }
}
