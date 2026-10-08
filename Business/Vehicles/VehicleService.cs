using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters.CRUD;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles;
public interface IVehicleService
{
    Task<Result<List<VehicleDto>>> GetAllVehiclesAsync();
    Task<Result<VehicleDto>> GetVehicleByIdAsync(Guid id);
    Task<Result<object>> CreateVehicleAsync(VehicleCreateDto vehicleCreate);
    Task<Result<object>> UpdateVehicleAsync(VehicleDto vehicle);
    Task<Result<object>> DeleteVehicleAsync(Guid id);
}

public class VehicleService(ApplicationDbContext context, IMapper mapper) : IVehicleService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<VehicleDto>>> GetAllVehiclesAsync()
    {
        try
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(p => p.Cult)
                .OrderBy(v => v.Name)
                .ToListAsync();
            return new Result<List<VehicleDto>> { Value = _mapper.Map<List<VehicleDto>>(vehicles) };
        }
        catch (Exception)
        {
            return new Result<List<VehicleDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<VehicleDto>> GetVehicleByIdAsync(Guid id)
    {
        try
        {
            var vehicle = await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(p => p.Cult)
            .FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle is null)
                return new Result<VehicleDto> { IsError = true , Error = "Vehicle not found" };

            return new Result<VehicleDto> { Value = _mapper.Map<VehicleDto>(vehicle) };
        }
        catch (Exception)
        {
            return new Result<VehicleDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateVehicleAsync(VehicleCreateDto vehicleCreate)
    {
        try
        {
            var vehicle = _mapper.Map<Vehicle>(vehicleCreate);

            var vehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Id == vehicleCreate.VehicleTypeId);
            if (vehicleType is null)
                return new Result<object> { IsError = true , Error = "Vehicle Type not found"};

            vehicle.VehicleType = vehicleType;

            if (vehicleCreate.CultId is not null && vehicleCreate.CultId != Guid.Empty)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == vehicleCreate.CultId);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
                vehicle.Cult = cult;
            }
            else
                vehicle.Cult = null;

            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateVehicleAsync(VehicleDto vehicleDto)
    {
        try
        {
            var existingVehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(p => p.Cult)
                .FirstOrDefaultAsync(v => v.Id == vehicleDto.Id);
            if (existingVehicle is null)
                return new Result<object> { IsError = true, Error = "Vehicle not found" };

            _mapper.Map(vehicleDto, existingVehicle);

            var vehicleType = await _context.VehicleTypes
                .FirstOrDefaultAsync(vt => vt.Id == vehicleDto.VehicleType.Id);
            if (vehicleType is null)
                return new Result<object> { IsError = true, Error = "Vehicle Type not found" };
            existingVehicle.VehicleType = vehicleType;


            if (vehicleDto.CultId is not null)
            {
                var cult = await _context.Cults
                    .FirstOrDefaultAsync(c => c.Id == vehicleDto.CultId);
                if (cult is null)
                    return new Result<object> { IsError = true, Error = "Cult not found" };
                existingVehicle.Cult = cult;
            }
            else
            {
                existingVehicle.Cult = null;
                existingVehicle.CultId = null;
            }

            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception) {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteVehicleAsync(Guid id)
    {
        try
        {
            var vehicle = await _context.Vehicles
                .Include(v => v.VehicleType)
                .Include(p => p.Cult)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (vehicle is null)
                return new Result<object> { IsError = true, Error = "Vehicle not found" };

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return new Result<object> { Value  = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}