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
            .Include(v => v.VehicleQualities)
            .Include(p => p.Cult)
            .ToListAsync();
        return _mapper.Map<List<VehicleDto>>(vehicles);
    }

    public async Task<VehicleDto?> GetVehicleByIdAsync(Guid id)
    {
        try
        {
            var vehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleQualities)
            .Include(p => p.Cult)
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

            foreach (var quality in vehicleCreate.VehicleQualities)
            {
                var existingQuality = _context.VehicleQualities.Find(quality.Id)
                    ?? throw new Exception("VehicleQuality not found");
                vehicle.VehicleQualities.Add(existingQuality);
            }

            if (vehicleCreate.CultId is not null && vehicleCreate.CultId != Guid.Empty)
            {
                vehicle.Cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == vehicleCreate.CultId)
                    ?? throw new Exception("Cult not found");
            }
            else
            {
                vehicle.Cult = null;
            }

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
                .Include(v => v.VehicleQualities)
                .Include(p => p.Cult)
                .FirstOrDefaultAsync(v => v.Id == vehicleDto.Id)
                ?? throw new Exception("Vehicle not found");

            _mapper.Map(vehicleDto, existingVehicle);
            existingVehicle.VehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Id == vehicleDto.VehicleType.Id)
                ?? throw new Exception("VehicleType not found");

            existingVehicle.VehicleQualities.Clear();
            foreach (var quality in vehicleDto.VehicleQualities)
            {
                var existingQuality = _context.VehicleQualities.Find(quality.Id)
                    ?? throw new Exception("VehicleQuality not found");
                existingVehicle.VehicleQualities.Add(existingQuality);
            }

            if (vehicleDto.CultId is not null)
            {
                existingVehicle.Cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == vehicleDto.CultId)
                    ?? throw new Exception("Cult not found");
            }
            else
            {
                existingVehicle.Cult = null;
                existingVehicle.CultId = null;
            }

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
                .Include(v => v.VehicleQualities)
                .Include(p => p.Cult)
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