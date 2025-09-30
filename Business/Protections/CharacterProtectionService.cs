//using DataAccessLayer;
//using Domain.Protections;
//using Microsoft.EntityFrameworkCore;

//namespace Business.Protections;
//public interface ICharacterProtectionService
//{
//    Task<List<CharacterProtection>> GetAllCharacterProtectionsAsync();
//    Task<CharacterProtection?> GetCharacterProtectionByIdAsync(Guid id);
//    Task<CharacterProtection> CreateCharacterProtectionAsync(CharacterProtection characterProtection);
//    Task<CharacterProtection?> UpdateCharacterProtectionAsync(Guid id, CharacterProtection characterProtection);
//    Task<bool> DeleteCharacterProtectionAsync(Guid id);
//}
//public class CharacterProtectionService : ICharacterProtectionService
//{
//    private readonly ApplicationDbContext _context;

//    public CharacterProtectionService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<List<CharacterProtection>> GetAllCharacterProtectionsAsync()
//    {
//        return await _context.CharacterProtections.ToListAsync();
//    }

//    public async Task<CharacterProtection?> GetCharacterProtectionByIdAsync(Guid id)
//    {
//        return await _context.CharacterProtections.FindAsync(id);
//    }

//    public async Task<CharacterProtection> CreateCharacterProtectionAsync(CharacterProtection characterProtection)
//    {
//        _context.CharacterProtections.Add(characterProtection);
//        await _context.SaveChangesAsync();
//        return characterProtection;
//    }

//    public async Task<CharacterProtection?> UpdateCharacterProtectionAsync(Guid id, CharacterProtection characterProtection)
//    {
//        var existingCharacterProtection = await _context.CharacterProtections.FindAsync(id);

//        if (existingCharacterProtection is null)
//        {
//            return null;
//        }

//        existingCharacterProtection = characterProtection;

//        await _context.SaveChangesAsync();
//        return existingCharacterProtection;
//    }

//    public async Task<bool> DeleteCharacterProtectionAsync(Guid id)
//    {
//        var existingCharacterProtection = await _context.CharacterProtections.FindAsync(id);

//        if (existingCharacterProtection is null)
//        {
//            return false;
//        }

//        _context.CharacterProtections.Remove(existingCharacterProtection);
//        await _context.SaveChangesAsync();
//        return true;
//    }
//}
