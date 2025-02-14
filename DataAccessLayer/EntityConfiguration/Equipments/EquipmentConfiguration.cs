using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Equipments;

namespace DataAccessLayer.EntityConfiguration.Equipments;

internal class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.ToTable("Equipments");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()")
            .HasColumnOrder(1);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(e => e.Capacity)
            .HasMaxLength(1000);

        builder.Property(e => e.Effect);
        builder.Property(e => e.Encumbrance);
        builder.Property(e => e.TechLevel);
        builder.Property(e => e.Slots);
        builder.Property(e => e.Value);
        builder.Property(e => e.Resources);
        builder.Property(e => e.EnergyStorage);

        builder.HasOne(e => e.EquipmentType)
            .WithMany()
            .HasForeignKey(e => e.EquipmentTypeId)
            .IsRequired();

    }
}