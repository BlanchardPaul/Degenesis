//using DataAccessLayer;
//using Domain.Equipments;
//using Microsoft.EntityFrameworkCore;

//namespace Business.Equipments;
//public interface ICharacterEquipmentService
//{
//    Task<List<CharacterEquipment>> GetAllCharacterEquipmentsAsync();
//    Task<CharacterEquipment?> GetCharacterEquipmentByIdAsync(Guid characterId, Guid equipmentId);
//    Task<CharacterEquipment> CreateCharacterEquipmentAsync(CharacterEquipment characterEquipment);
//    Task<CharacterEquipment?> UpdateCharacterEquipmentAsync(Guid characterId, Guid equipmentId, CharacterEquipment characterEquipment);
//    Task<bool> DeleteCharacterEquipmentAsync(Guid characterId, Guid equipmentId);
//}
//public class CharacterEquipmentService : ICharacterEquipmentService
//{
//    private readonly ApplicationDbContext _context;

//    public CharacterEquipmentService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<CharacterEquipment>> GetAllCharacterEquipmentsAsync()
//    {
//        return await _context.CharacterEquipments
//            .Include(ce => ce.Character)
//            .Include(ce => ce.Equipment)
//            .ToListAsync();
//    }

//    public async Task<CharacterEquipment?> GetCharacterEquipmentByIdAsync(Guid characterId, Guid equipmentId)
//    {
//        return await _context.CharacterEquipments
//            .Include(ce => ce.Character)
//            .Include(ce => ce.Equipment)
//            .FirstOrDefaultAsync(ce => ce.CharacterId == characterId && ce.EquipmentId == equipmentId);
//    }

//    public async Task<CharacterEquipment> CreateCharacterEquipmentAsync(CharacterEquipment characterEquipment)
//    {
//        _context.CharacterEquipments.Add(characterEquipment);
//        await _context.SaveChangesAsync();
//        return characterEquipment;
//    }

//    public async Task<CharacterEquipment?> UpdateCharacterEquipmentAsync(Guid characterId, Guid equipmentId, CharacterEquipment characterEquipment)
//    {
//        var existingCharacterEquipment = await _context.CharacterEquipments
//            .FirstOrDefaultAsync(ce => ce.CharacterId == characterId && ce.EquipmentId == equipmentId);

//        if (existingCharacterEquipment is null)
//        {
//            return null;
//        }

//        existingCharacterEquipment = characterEquipment;

//        await _context.SaveChangesAsync();
//        return existingCharacterEquipment;
//    }

//    public async Task<bool> DeleteCharacterEquipmentAsync(Guid characterId, Guid equipmentId)
//    {
//        var existingCharacterEquipment = await _context.CharacterEquipments
//            .FirstOrDefaultAsync(ce => ce.CharacterId == characterId && ce.EquipmentId == equipmentId);

//        if (existingCharacterEquipment is null)
//        {
//            return false;
//        }

//        _context.CharacterEquipments.Remove(existingCharacterEquipment);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}
