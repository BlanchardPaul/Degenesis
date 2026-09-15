using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters.Inventory;
namespace DataAccessLayer.EntityConfiguration.Characters.Inventory;
internal class CharacterWeaponConfiguration : IEntityTypeConfiguration<CharacterWeapon>
{
    public void Configure(EntityTypeBuilder<CharacterWeapon> builder)
    {
        builder.ToTable("CharacterWeapons");

        builder.HasKey(cw => cw.Id);

        builder.Property(cw => cw.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cw => cw.BulletsInMagazine);
        builder.Property(cw => cw.UsedSlots);
        builder.Property(cw => cw.Slots);
        builder.Property(cw => cw.Encumbrance);
        builder.Property(cw => cw.Qualities);

        builder.HasOne(cw => cw.Character)
            .WithMany(c => c.CharacterWeapons)
            .HasForeignKey(cw => cw.CharacterId);

        builder.HasOne(cw => cw.Weapon)
            .WithMany()
            .HasForeignKey(cw => cw.WeaponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}