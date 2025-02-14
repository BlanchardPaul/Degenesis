using Domain.Vehicles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Vehicles;
internal class VehicleTypeConfiguration : IEntityTypeConfiguration<VehicleType>
{
    public void Configure(EntityTypeBuilder<VehicleType> builder)
    {
        builder.ToTable("VehicleTypes");

        builder.HasKey(vt => vt.Id);

        builder.Property(vt => vt.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(vt => vt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vt => vt.Description)
            .IsRequired()
            .HasMaxLength(1000);
    }
}