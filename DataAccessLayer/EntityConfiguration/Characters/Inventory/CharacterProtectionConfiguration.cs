using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters.Inventory;

namespace DataAccessLayer.EntityConfiguration.Characters.Inventory;
internal class CharacterProtectionConfiguration : IEntityTypeConfiguration<CharacterProtection>
{
    public void Configure(EntityTypeBuilder<CharacterProtection> builder)
    {
        builder.ToTable("CharacterProtections");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cp => cp.UsedSlots);

        builder.Property(cp => cp.Slots);

        builder.Property(cp => cp.Qualities);

        builder.Property(cp => cp.Encumbrance);

        builder.HasOne(cp => cp.Character)
            .WithMany(c => c.CharacterProtections)
            .HasForeignKey(cp => cp.CharacterId);

        builder.HasOne(cp => cp.Protection)
            .WithMany()
            .HasForeignKey(cp => cp.ProtectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}