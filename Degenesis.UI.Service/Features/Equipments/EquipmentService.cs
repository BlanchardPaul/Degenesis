using Degenesis.Shared.DTOs.Equipments;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Equipments;

public class EquipmentService
{
    private readonly HttpClient _httpClient;

    public EquipmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EquipmentDto>> GetEquipmentsAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<EquipmentDto>>("/equipments") ?? new();
    }

    public async Task<EquipmentDto> CreateEquipmentAsync(EquipmentCreateDto equipment)
    {
        var response = await _httpClient.PostAsJsonAsync("/equipments", equipment);
        return await response.Content.ReadFromJsonAsync<EquipmentDto>();
    }

    public async Task UpdateEquipmentAsync(EquipmentDto equipment)
    {
        await _httpClient.PutAsJsonAsync($"/equipments", equipment);
    }

    public async Task DeleteEquipmentAsync(Guid equipmentId)
    {
        await _httpClient.DeleteAsync($"/equipments/{equipmentId}");
    }
}