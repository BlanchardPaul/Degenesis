using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Equipments;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles;
public interface IVehicleService
{
    Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync();
    Task<VehicleDto?> GetVehicleByIdAsync(Guid id);
    Task<VehicleDto?> CreateVehicleAsync(VehicleCreateDto vehicleCreate);
    Task<bool> UpdateVehicleAsync(VehicleDto vehicle);
    Task<bool> DeleteVehicleAsync(Guid id);
}

public class VehicleService : IVehicleService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public VehicleService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehicleDto>> GetAllVehiclesAsync()
    {
        var vehicles = await _context.Vehicles
            .Include(v => v.VehicleType)
            .ToListAsync();
        return _mapper.Map<IEnumerable<VehicleDto>>(vehicles);
    }

    public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == id);

        return vehicle is null ? null : _mapper.Map<VehicleDto>(vehicle);
    }

    public async Task<VehicleDto?> CreateVehicleAsync(VehicleCreateDto vehicleCreate)
    {
        try
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleCreate);
            vehicle.VehicleType = await _context.VehicleTypes.FirstAsync(vt => vt.Id == vehicleCreate.VehicleTypeId);

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleDto>(vehicle);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateVehicleAsync(VehicleDto vehicleDto)
    {
        try
        {
            var existingVehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == vehicleDto.Id);

            if (existingVehicle is null || vehicleDto.VehicleType is null)
                return false;

            _mapper.Map(vehicleDto, existingVehicle);
            existingVehicle.VehicleType = await _context.VehicleTypes.FirstAsync(vt => vt.Id == vehicleDto.VehicleType.Id);

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    public async Task<bool> DeleteVehicleAsync(Guid id)
    {
        try
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle is null)
                return false;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}