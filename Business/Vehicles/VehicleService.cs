using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles;
public interface IVehicleService
{
    Task<List<VehicleDto>> GetAllVehiclesAsync();
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

    public async Task<List<VehicleDto>> GetAllVehiclesAsync()
    {
        var vehicles = await _context.Vehicles
            .Include(v => v.VehicleType)
            .ToListAsync();
        return _mapper.Map<List<VehicleDto>>(vehicles);
    }

    public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
    {
        try
        {
            var vehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == id)
            ?? throw new Exception("Vehicle not found");

            return _mapper.Map<VehicleDto>(vehicle);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<VehicleDto?> CreateVehicleAsync(VehicleCreateDto vehicleCreate)
    {
        try
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleCreate);
            vehicle.VehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Id == vehicleCreate.VehicleTypeId)
                ?? throw new Exception("VehicleType not found");

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
                .FirstOrDefaultAsync(v => v.Id == vehicleDto.Id)
                ?? throw new Exception("Vehicle not found");

            _mapper.Map(vehicleDto, existingVehicle);
            existingVehicle.VehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Id == vehicleDto.VehicleType.Id)
                ?? throw new Exception("VehicleType not found");

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
            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .FirstOrDefaultAsync(v => v.Id == id)
                ?? throw new Exception("Vehicle not found");

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