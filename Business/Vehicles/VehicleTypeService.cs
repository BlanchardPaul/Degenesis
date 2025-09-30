using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles; 
public interface IVehicleTypeService
{
    Task<List<VehicleTypeDto>> GetAllVehicleTypesAsync();
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

    public async Task<List<VehicleTypeDto>> GetAllVehicleTypesAsync()
    {
        var vehicleTypes = await _context.VehicleTypes.ToListAsync();
        return _mapper.Map<List<VehicleTypeDto>>(vehicleTypes);
    }

    public async Task<VehicleTypeDto?> GetVehicleTypeByIdAsync(Guid id)
    {
        try
        {
            var vehicleType = await _context.VehicleTypes.FindAsync(id)
            ?? throw new Exception("VehicleType not found");

            return _mapper.Map<VehicleTypeDto>(vehicleType);
        }
        catch (Exception)
        {
            return null;
        }
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
            var existingVehicleType = await _context.VehicleTypes.FindAsync(vehicleTypeDto.Id)
                ?? throw new Exception("VehicleType not found");

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
            var vehicleType = await _context.VehicleTypes.FindAsync(id)
                ?? throw new Exception("VehicleType not found");

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