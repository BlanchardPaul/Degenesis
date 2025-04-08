using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles; 
public interface IVehicleTypeService
{
    Task<IEnumerable<VehicleTypeDto>> GetAllVehicleTypesAsync();
    Task<VehicleTypeDto?> GetVehicleTypeByIdAsync(Guid id);
    Task<VehicleTypeDto?> CreateVehicleTypeAsync(VehicleTypeCreateDto vehicleTypeCreate);
    Task<bool> UpdateVehicleTypeAsync(VehicleTypeDto vehicleType);
    Task<bool> DeleteVehicleTypeAsync(Guid id);
}

public class VehicleTypeService : IVehicleTypeService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public VehicleTypeService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehicleTypeDto>> GetAllVehicleTypesAsync()
    {
        var vehicleTypes = await _context.VehicleTypes.ToListAsync();
        return _mapper.Map<IEnumerable<VehicleTypeDto>>(vehicleTypes);
    }

    public async Task<VehicleTypeDto?> GetVehicleTypeByIdAsync(Guid id)
    {
        var vehicleType = await _context.VehicleTypes.FindAsync(id);
        return vehicleType is null ? null : _mapper.Map<VehicleTypeDto>(vehicleType);
    }

    public async Task<VehicleTypeDto?> CreateVehicleTypeAsync(VehicleTypeCreateDto vehicleTypeCreate)
    {
        try
        {
            var vehicleType = _mapper.Map<VehicleType>(vehicleTypeCreate);
            _context.VehicleTypes.Add(vehicleType);
            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleTypeDto>(vehicleType);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateVehicleTypeAsync(VehicleTypeDto vehicleTypeDto)
    {
        try
        {
            var existingVehicleType = await _context.VehicleTypes.FindAsync(vehicleTypeDto.Id);
            if (existingVehicleType is null)
                return false;

            _mapper.Map(vehicleTypeDto, existingVehicleType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteVehicleTypeAsync(Guid id)
    {
        try
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id);
            if (vehicleType is null)
                return false;

            _context.VehicleTypes.Remove(vehicleType);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}