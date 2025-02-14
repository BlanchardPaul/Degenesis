using Domain.Weapons;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Weapons;
internal class NPCWeaponConfiguration : IEntityTypeConfiguration<NPCWeapon>
{
    public void Configure(EntityTypeBuilder<NPCWeapon> builder)
    {
        builder.ToTable("NPCWeapons");

        builder.HasKey(nw => nw.Id);

        builder.Property(nw => nw.BulletsInMagazine)
            .IsRequired();

        builder.Property(nw => nw.UsedSlots)
            .IsRequired();

        builder.Property(nw => nw.SlotAttachments)
            .IsRequired();

        builder.HasOne(nw => nw.NPC)
            .WithMany(n => n.NPCWeapons)
            .HasForeignKey(nw => nw.NPCId);

        builder.HasOne(nw => nw.Weapon)
            .WithMany()
            .HasForeignKey(nw => nw.WeaponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}