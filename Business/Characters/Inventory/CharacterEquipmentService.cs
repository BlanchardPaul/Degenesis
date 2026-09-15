using AutoMapper;
using DataAccessLayer;
using Degenesis.Shared.DTOs.Characters.CRUD.Inventory;
using Domain.Characters.Inventory;
using Microsoft.EntityFrameworkCore;

namespace Business.Characters.Inventory;

public interface ICharacterEquipmentService
{
    Task<CharacterEquipmentDto?> GetByIdAsync(Guid id);
    Task<List<CharacterEquipmentDto>> GetByCharacterIdAsync(Guid characterId);
    Task<CharacterEquipmentDto?> CreateAsync(CharacterEquipmentCreateDto characterEquipment);
    Task<bool> UpdateAsync(CharacterEquipmentDto characterEquipment);
    Task<bool> DeleteAsync(Guid id);
}

public class CharacterEquipmentService : ICharacterEquipmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CharacterEquipmentService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CharacterEquipmentDto?> GetByIdAsync(Guid id)
    {
        try
        {
            var characterEquipment = await _context.CharacterEquipments
                .Include(ca => ca.Equipment)
                .FirstOrDefaultAsync(ca => ca.Id == id) ?? throw new Exception("CharacterEquipment not found");
            return _mapper.Map<CharacterEquipmentDto>(characterEquipment);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<CharacterEquipmentDto>> GetByCharacterIdAsync(Guid characterId)
    {
        var characterEquipments =
            await _context.CharacterEquipments
            .Where(ca => ca.CharacterId == characterId)
            .Include(ca => ca.Equipment)
                .ThenInclude(e => e.EquipmentType)
            .ToListAsync();
        return _mapper.Map<List<CharacterEquipmentDto>>(characterEquipments);
    }

    public async Task<CharacterEquipmentDto?> CreateAsync(CharacterEquipmentCreateDto characterEquipmentCreate)
    {
        try
        {
            var characterEquipment = _mapper.Map<CharacterEquipment>(characterEquipmentCreate);

            characterEquipment.Character = await _context.Characters.FindAsync(characterEquipmentCreate.CharacterId) 
                ?? throw new Exception("Character not found");
            characterEquipment.Equipment = await _context.Equipments.FindAsync(characterEquipmentCreate.EquipmentId) 
                ?? throw new Exception("Equipment not found");

            _context.CharacterEquipments.Add(characterEquipment);

            await _context.SaveChangesAsync();
            return _mapper.Map<CharacterEquipmentDto>(characterEquipment);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateAsync(CharacterEquipmentDto characterEquipment)
    {
        try
        {
            var existing = await _context.CharacterEquipments.FirstOrDefaultAsync(ca => ca.Id == characterEquipment.Id) 
                ?? throw new Exception("CharacterEquipment not found");
            _mapper.Map(characterEquipment, existing);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            var characterEquipment = await _context.CharacterEquipments.FirstOrDefaultAsync(ca => ca.Id == id) ?? throw new Exception("CharacterEquipment not found");

            _context.CharacterEquipments.Remove(characterEquipment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}