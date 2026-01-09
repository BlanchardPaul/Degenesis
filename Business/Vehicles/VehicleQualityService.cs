using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Vehicles;
using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Business.Vehicles;

public interface IVehicleQualityService
{
    Task<IEnumerable<VehicleQualityDto>> GetAllVehicleQualitiesAsync();
    Task<VehicleQualityDto?> GetVehicleQualityByIdAsync(Guid id);
    Task<VehicleQualityDto?> CreateVehicleQualityAsync(VehicleQualityCreateDto vehicleQualityCreate);
    Task<bool> UpdateVehicleQualityAsync(VehicleQualityDto vehicleQuality);
    Task<bool> DeleteVehicleQualityAsync(Guid id);
}

public class VehicleQualityService : IVehicleQualityService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public VehicleQualityService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehicleQualityDto>> GetAllVehicleQualitiesAsync()
    {
        var vehicleQualities = await _context.VehicleQualities.ToListAsync();
        return _mapper.Map<IEnumerable<VehicleQualityDto>>(vehicleQualities);
    }

    public async Task<VehicleQualityDto?> GetVehicleQualityByIdAsync(Guid id)
    {
        try
        {
            var vehicleQuality = await _context.VehicleQualities.FindAsync(id)
                ?? throw new Exception("VehicleQuality not found");
            return _mapper.Map<VehicleQualityDto>(vehicleQuality);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<VehicleQualityDto?> CreateVehicleQualityAsync(VehicleQualityCreateDto vehicleQualityCreate)
    {
        try
        {
            var vehicleQuality = _mapper.Map<VehicleQuality>(vehicleQualityCreate);
            _context.VehicleQualities.Add(vehicleQuality);
            await _context.SaveChangesAsync();
            return _mapper.Map<VehicleQualityDto>(vehicleQuality);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateVehicleQualityAsync(VehicleQualityDto vehicleQualityDto)
    {
        try
        {
            var existingVehicleQuality = await _context.VehicleQualities.FindAsync(vehicleQualityDto.Id)
                ?? throw new Exception("VehicleQuality not found");

            _mapper.Map(vehicleQualityDto, existingVehicleQuality);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteVehicleQualityAsync(Guid id)
    {
        try
        {
            var vehicleQuality = await _context.VehicleQualities.FindAsync(id)
                ?? throw new Exception("VehicleQuality not found");

            _context.VehicleQualities.Remove(vehicleQuality);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}