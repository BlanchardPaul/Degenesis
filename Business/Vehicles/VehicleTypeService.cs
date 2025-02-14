using DataAccessLayer;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles;
public interface IVehicleTypeService
{
    Task<List<VehicleType>> GetAllVehicleTypesAsync();
    Task<VehicleType?> GetVehicleTypeByIdAsync(Guid id);
    Task<VehicleType> CreateVehicleTypeAsync(VehicleType vehicleType);
    Task<VehicleType?> UpdateVehicleTypeAsync(Guid id, VehicleType vehicleType);
    Task<bool> DeleteVehicleTypeAsync(Guid id);
}
public class VehicleTypeService : IVehicleTypeService
{
    private readonly ApplicationDbContext _context;

    public VehicleTypeService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<VehicleType>> GetAllVehicleTypesAsync()
    {
        return await _context.VehicleTypes.ToListAsync();
    }

    public async Task<VehicleType?> GetVehicleTypeByIdAsync(Guid id)
    {
        return await _context.VehicleTypes.FindAsync(id);
    }

    public async Task<VehicleType> CreateVehicleTypeAsync(VehicleType vehicleType)
    {
        _context.VehicleTypes.Add(vehicleType);
        await _context.SaveChangesAsync();
        return vehicleType;
    }

    public async Task<VehicleType?> UpdateVehicleTypeAsync(Guid id, VehicleType vehicleType)
    {
        var existingVehicleType = await _context.VehicleTypes.FindAsync(id);

        if (existingVehicleType == null)
        {
            return null;
        }

        existingVehicleType = vehicleType;

        await _context.SaveChangesAsync();
        return existingVehicleType;
    }

    public async Task<bool> DeleteVehicleTypeAsync(Guid id)
    {
        var existingVehicleType = await _context.VehicleTypes.FindAsync(id);

        if (existingVehicleType == null)
        {
            return false;
        }

        _context.VehicleTypes.Remove(existingVehicleType);
        await _context.SaveChangesAsync();
        return true;
    }
}
