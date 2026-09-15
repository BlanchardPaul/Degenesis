using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters.Inventory;

namespace DataAccessLayer.EntityConfiguration.Characters.Inventory;
internal class CharacterVehicleConfiguration : IEntityTypeConfiguration<CharacterVehicle>
{
    public void Configure(EntityTypeBuilder<CharacterVehicle> builder)
    {
        builder.ToTable("CharacterVehicles");

        builder.HasKey(cv => cv.Id);

        builder.Property(cv => cv.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cv => cv.UsedSlots);
        builder.Property(cv => cv.Slots);
        builder.Property(cv => cv.Qualities);
        builder.Property(cv => cv.BodyFlesh);
        builder.Property(cv => cv.StructureTrauma);
        builder.Property(cv => cv.BodyFleshLost);
        builder.Property(cv => cv.StructureTraumaLost);

        builder.HasOne(cv => cv.Character)
            .WithMany(c => c.CharacterVehicles)
            .HasForeignKey(cv => cv.CharacterId);

        builder.HasOne(cv => cv.Vehicle)
            .WithMany()
            .HasForeignKey(cv => cv.VehicleId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}