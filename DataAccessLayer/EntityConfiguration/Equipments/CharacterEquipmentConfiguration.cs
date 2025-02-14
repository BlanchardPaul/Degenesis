using Domain.Equipments;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Equipments;
internal class CharacterEquipmentConfiguration : IEntityTypeConfiguration<CharacterEquipment>
{
    public void Configure(EntityTypeBuilder<CharacterEquipment> builder)
    {
        builder.ToTable("CharacterEquipments");

        builder.HasKey(ce => ce.Id);

        builder.Property(ce => ce.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(ce => ce.UsedSlots)
            .IsRequired();

        builder.HasOne(ce => ce.Character)
            .WithMany(c => c.CharacterEquipments)
            .HasForeignKey(ce => ce.CharacterId);

        builder.HasOne(ce => ce.Equipment)
            .WithMany()
            .HasForeignKey(ce => ce.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}