using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs;
using Degenesis.Shared.DTOs.Characters;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles; 
public interface IVehicleTypeService
{
    Task<Result<List<VehicleTypeDto>>> GetAllVehicleTypesAsync();
    Task<Result<VehicleTypeDto>> GetVehicleTypeByIdAsync(Guid id);
    Task<Result<object>> CreateVehicleTypeAsync(VehicleTypeCreateDto vehicleTypeCreate);
    Task<Result<object>> UpdateVehicleTypeAsync(VehicleTypeDto vehicleType);
    Task<Result<object>> DeleteVehicleTypeAsync(Guid id);
}

public class VehicleTypeService(ApplicationDbContext context, IMapper mapper) : IVehicleTypeService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<List<VehicleTypeDto>>> GetAllVehicleTypesAsync()
    {
        try
        {
            var vehicleTypes = await _context.VehicleTypes.OrderBy(v => v.Name).ToListAsync();
            return new Result<List<VehicleTypeDto>> { Value = _mapper.Map<List<VehicleTypeDto>>(vehicleTypes) };
        }
        catch (Exception)
        {
            return new Result<List<VehicleTypeDto>> { IsError = true, Error = "A server error occurred" };
        }

    }

    public async Task<Result<VehicleTypeDto>> GetVehicleTypeByIdAsync(Guid id)
    {
        try
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if ( vehicleType is null)
                return new Result<VehicleTypeDto> { IsError = true, Error = "Vehicle Type not found" };

            return new Result<VehicleTypeDto> { Value = _mapper.Map<VehicleTypeDto>(vehicleType) };
        }
        catch (Exception)
        {
            return new Result<VehicleTypeDto> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> CreateVehicleTypeAsync(VehicleTypeCreateDto vehicleTypeCreate)
    {
        try
        {
            var vehicleType = _mapper.Map<VehicleType>(vehicleTypeCreate);
            _context.VehicleTypes.Add(vehicleType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> UpdateVehicleTypeAsync(VehicleTypeDto vehicleTypeDto)
    {
        try
        {
            var existingVehicleType = await _context.VehicleTypes.FindAsync(vehicleTypeDto.Id);
            if ( existingVehicleType is null)
                return new Result<object> { IsError = true, Error = "Vehicle Type not found" };

            _mapper.Map(vehicleTypeDto, existingVehicleType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }

    public async Task<Result<object>> DeleteVehicleTypeAsync(Guid id)
    {
        try
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType is null)
                return new Result<object> { IsError = true, Error = "Vehicle Type not found" };

            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return new Result<object> { Value = null };
        }
        catch (Exception)
        {
            return new Result<object> { IsError = true, Error = "A server error occurred" };
        }
    }
}