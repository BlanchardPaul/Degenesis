using Domain.Vehicles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Vehicles;
internal class CharacterVehicleConfiguration : IEntityTypeConfiguration<CharacterVehicle>
{
    public void Configure(EntityTypeBuilder<CharacterVehicle> builder)
    {
        builder.ToTable("CharacterVehicles");

        builder.HasKey(cv => cv.Id);

        builder.Property(cv => cv.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cv => cv.UsedSlots)
            .IsRequired();

        builder.Property(cv => cv.FleshLost)
            .IsRequired();

        builder.Property(cv => cv.TraumaLost)
            .IsRequired();

        builder.HasOne(cv => cv.Character)
            .WithMany(c => c.CharacterVehicles)
            .HasForeignKey(cv => cv.CharacterId);

        builder.HasOne(cv => cv.Vehicle)
            .WithMany()
            .HasForeignKey(cv => cv.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}