using Degenesis.Shared.DTOs.Vehicles;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Vehicles;
public class VehicleTypeService
{
    private readonly HttpClient _httpClient;

    public VehicleTypeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<VehicleTypeDto>> GetVehicleTypesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<VehicleTypeDto>>("/vehicle-types") ?? new();
    }

    public async Task<VehicleTypeDto> CreateVehicleTypeAsync(VehicleTypeCreateDto vehicleType)
    {
        var response = await _httpClient.PostAsJsonAsync("/vehicle-types", vehicleType);
        return await response.Content.ReadFromJsonAsync<VehicleTypeDto>();
    }

    public async Task UpdateVehicleTypeAsync(VehicleTypeDto vehicleType)
    {
        await _httpClient.PutAsJsonAsync($"/vehicle-types", vehicleType);
    }

    public async Task DeleteVehicleTypeAsync(Guid vehicleTypeId)
    {
        await _httpClient.DeleteAsync($"/vehicle-types/{vehicleTypeId}");
    }
}
