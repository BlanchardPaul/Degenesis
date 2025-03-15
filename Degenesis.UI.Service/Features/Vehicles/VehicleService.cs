using Degenesis.Shared.DTOs.Vehicles;
using System.Net.Http.Json;

namespace Degenesis.UI.Service.Features.Vehicles;
public class VehicleService
{
    private readonly HttpClient _httpClient;

    public VehicleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<VehicleDto>> GetVehiclesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<VehicleDto>>("/vehicles") ?? new();
    }

    public async Task<VehicleDto> CreateVehicleAsync(VehicleCreateDto vehicle)
    {
        var response = await _httpClient.PostAsJsonAsync("/vehicles", vehicle);
        return await response.Content.ReadFromJsonAsync<VehicleDto>();
    }

    public async Task UpdateVehicleAsync(VehicleDto vehicle)
    {
        await _httpClient.PutAsJsonAsync($"/vehicles", vehicle);
    }

    public async Task DeleteVehicleAsync(Guid vehicleId)
    {
        await _httpClient.DeleteAsync($"/vehicles/{vehicleId}");
    }
}
