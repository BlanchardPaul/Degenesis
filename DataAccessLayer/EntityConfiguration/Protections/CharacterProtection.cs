using Domain.Protections;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Protections;
internal class CharacterProtectionConfiguration : IEntityTypeConfiguration<CharacterProtection>
{
    public void Configure(EntityTypeBuilder<CharacterProtection> builder)
    {
        builder.ToTable("CharacterProtections");

        builder.HasKey(cp => cp.Id);

        builder.Property(cp => cp.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(cp => cp.UsedConnectors)
            .IsRequired();

        builder.Property(cp => cp.UsedSlots)
            .IsRequired();

        builder.HasOne(cp => cp.Character)
            .WithMany(c => c.CharacterProtections)
            .HasForeignKey(cp => cp.CharacterId);

        builder.HasOne(cp => cp.Protection)
            .WithMany()
            .HasForeignKey(cp => cp.ProtectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}