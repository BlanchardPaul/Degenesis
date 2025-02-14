using Domain.Vehicles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityConfiguration.Vehicles;
internal class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(v => v.MaxSpeed).IsRequired();
        builder.Property(v => v.Acceleration).IsRequired();
        builder.Property(v => v.Brake).IsRequired();
        builder.Property(v => v.Armor).IsRequired();
        builder.Property(v => v.BodyFlesh).IsRequired();
        builder.Property(v => v.StrucutureTrauma).IsRequired();
        builder.Property(v => v.TechLevel).IsRequired();
        builder.Property(v => v.Slots).IsRequired();
        builder.Property(v => v.Value).IsRequired();

        builder.Property(v => v.Resources)
            .IsRequired();

        builder.HasOne(v => v.VehicleType)
            .WithMany()
            .HasForeignKey(v => v.VehicleTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
