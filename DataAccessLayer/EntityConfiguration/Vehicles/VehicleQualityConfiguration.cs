using Domain.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Vehicles;

internal class VehicleQualityConfiguration : IEntityTypeConfiguration<VehicleQuality>
{
    public void Configure(EntityTypeBuilder<VehicleQuality> builder)
    {
        builder.ToTable("VehicleQualities");

        builder.HasKey(vq => vq.Id);

        builder.Property(vq => vq.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(vq => vq.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vq => vq.Description)
            .IsRequired();
    }
}