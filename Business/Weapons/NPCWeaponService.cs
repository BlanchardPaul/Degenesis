//using DataAccessLayer;
//using Domain.Weapons;
//using Microsoft.EntityFrameworkCore;

//namespace Business.Weapons;
//public interface INPCWeaponService
//{
//    Task<NPCWeapon?> GetNPCWeaponByIdAsync(Guid id);
//    Task<NPCWeapon?> GetNPCWeaponByNPCIdAsync(Guid npcId);
//    Task CreateNPCWeaponAsync(NPCWeapon npcWeapon);
//    Task UpdateNPCWeaponAsync(Guid id, NPCWeapon npcWeapon);
//    Task DeleteNPCWeaponAsync(Guid id);
//}

//public class NPCWeaponService : INPCWeaponService
//{
//    private readonly ApplicationDbContext _context;

//    public NPCWeaponService(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public async Task<NPCWeapon?> GetNPCWeaponByIdAsync(Guid id)
//    {
//        return await _context.NPCWeapons
//                             .Include(nw => nw.Weapon)
//                             .FirstOrDefaultAsync(nw => nw.Id == id);
//    }

//    public async Task<NPCWeapon?> GetNPCWeaponByNPCIdAsync(Guid npcId)
//    {
//        return await _context.NPCWeapons
//                             .Include(nw => nw.Weapon)
//                             .FirstOrDefaultAsync(nw => nw.NPCId == npcId);
//    }

//    public async Task CreateNPCWeaponAsync(NPCWeapon npcWeapon)
//    {
//        _context.NPCWeapons.Add(npcWeapon);
//        await _context.SaveChangesAsync();
//    }

//    public async Task UpdateNPCWeaponAsync(Guid id, NPCWeapon npcWeapon)
//    {
//        var existingNPCWeapon = await _context.NPCWeapons.FindAsync(id);
//        if (existingNPCWeapon != null)
//        {
//            existingNPCWeapon = npcWeapon;
//            await _context.SaveChangesAsync();
//        }
//    }

//    public async Task DeleteNPCWeaponAsync(Guid id)
//    {
//        var npcWeapon = await _context.NPCWeapons.FindAsync(id);
//        if (npcWeapon != null)
//        {
//            _context.NPCWeapons.Remove(npcWeapon);
//            await _context.SaveChangesAsync();
//        }
//    }
//}
