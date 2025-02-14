using Domain.Weapons;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityConfiguration.Weapons;
internal class CharacterWeaponConfiguration : IEntityTypeConfiguration<CharacterWeapon>
{
    public void Configure(EntityTypeBuilder<CharacterWeapon> builder)
    {
        builder.ToTable("CharacterWeapons");

        builder.HasKey(cw => cw.Id);

        builder.Property(cw => cw.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cw => cw.BulletsInMagazine)
            .IsRequired();

        builder.Property(cw => cw.UsedSlots)
            .IsRequired();

        builder.Property(cw => cw.SlotAttachments)
            .IsRequired();

        builder.HasOne(cw => cw.Character)
            .WithMany(c => c.CharacterWeapons)
            .HasForeignKey(cw => cw.CharacterId);

        builder.HasOne(cw => cw.Weapon)
            .WithMany()
            .HasForeignKey(cw => cw.WeaponId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}