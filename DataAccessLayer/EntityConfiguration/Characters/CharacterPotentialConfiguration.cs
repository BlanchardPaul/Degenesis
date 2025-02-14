using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;

internal sealed class CharacterPotentialConfiguration : IEntityTypeConfiguration<CharacterPotential>
{
    public void Configure(EntityTypeBuilder<CharacterPotential> builder)
    {
        builder.ToTable("CharacterPotentials");
        builder.HasKey(cp => new { cp.CharacterId, cp.PotentialId });

        builder.HasOne(cp => cp.Character)
            .WithMany(c => c.CharacterPontentials)
            .HasForeignKey(c => c.CharacterId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(cp => cp.Potential)
            .WithMany()
            .HasForeignKey(b => b.PotentialId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(cp => cp.Level)
            .IsRequired()
            .HasMaxLength(100);
    }
}