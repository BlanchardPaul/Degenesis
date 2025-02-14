using Domain.Equipments;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Equipments;
internal class EquipmentTypeConfiguration : IEntityTypeConfiguration<EquipmentType>
{
    public void Configure(EntityTypeBuilder<EquipmentType> builder)
    {
        builder.ToTable("EquipmentTypes");

        builder.HasKey(et => et.Id);

        builder.Property(et => et.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(et => et.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(et => et.Description)
            .IsRequired()
            .HasMaxLength(1000);
    }
}