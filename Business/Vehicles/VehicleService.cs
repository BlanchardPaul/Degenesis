using DataAccessLayer;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles;
public interface IVehicleService
{
    Task<List<Vehicle>> GetAllVehiclesAsync();
    Task<Vehicle?> GetVehicleByIdAsync(Guid id);
    Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
    Task<Vehicle?> UpdateVehicleAsync(Guid id, Vehicle vehicle);
    Task<bool> DeleteVehicleAsync(Guid id);
}
public class VehicleService : IVehicleService
{
    private readonly ApplicationDbContext _context;

    public VehicleService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehicle>> GetAllVehiclesAsync()
    {
        return await _context.Vehicles.ToListAsync();
    }

    public async Task<Vehicle?> GetVehicleByIdAsync(Guid id)
    {
        return await _context.Vehicles.FindAsync(id);
    }

    public async Task<Vehicle> CreateVehicleAsync(Vehicle vehicle)
    {
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();
        return vehicle;
    }

    public async Task<Vehicle?> UpdateVehicleAsync(Guid id, Vehicle vehicle)
    {
        var existingVehicle = await _context.Vehicles.FindAsync(id);

        if (existingVehicle == null)
        {
            return null;
        }

        existingVehicle = vehicle;

        await _context.SaveChangesAsync();
        return existingVehicle;
    }

    public async Task<bool> DeleteVehicleAsync(Guid id)
    {
        var existingVehicle = await _context.Vehicles.FindAsync(id);

        if (existingVehicle == null)
        {
            return false;
        }

        _context.Vehicles.Remove(existingVehicle);
        await _context.SaveChangesAsync();
        return true;
    }
}
