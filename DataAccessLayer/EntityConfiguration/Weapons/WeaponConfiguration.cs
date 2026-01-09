using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Weapons;

namespace DataAccessLayer.EntityConfiguration.Weapons;

internal class WeaponConfiguration : IEntityTypeConfiguration<Weapon>
{
    public void Configure(EntityTypeBuilder<Weapon> builder)
    {
        builder.ToTable("Weapons");
        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id)
            .HasDefaultValueSql("NEWID()")
            .HasColumnOrder(1);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(w => w.Description)
            .IsRequired();

        builder.Property(w => w.Caliber)
            .HasMaxLength(1000);

        builder.Property(w => w.Handling)
            .HasMaxLength(1000);

        builder.Property(w => w.Distance)
            .HasMaxLength(30);

        builder.Property(w => w.Damage);

        builder.HasOne(w => w.Attribute)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(w => w.AttributeId);

        builder.HasOne(w => w.Skill)
            .WithMany()
            .IsRequired(false)
            .HasForeignKey(w => w.SkillId);

        builder.Property(w => w.CharacterAttributeModifier);
        builder.Property(w => w.Magazine);
        builder.Property(w => w.Encumbrance);
        builder.Property(w => w.TechLevel);
        builder.Property(w => w.Slots);
        builder.Property(w => w.Value);
        builder.Property(w => w.Resources);

        builder.HasOne(w => w.WeaponType)
           .WithMany()
           .IsRequired()
           .HasForeignKey(w => w.WeaponTypeId);

        builder.HasMany(w => w.Qualities)
            .WithMany();

        builder.HasMany(w => w.Cults)
            .WithMany();
    }
}