using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;

internal sealed class CharacterBackgroundConfiguration : IEntityTypeConfiguration<CharacterBackground>
{
    public void Configure(EntityTypeBuilder<CharacterBackground> builder)
    {
        builder.ToTable("CharacterBackgrounds");
        builder.HasKey(cb => new { cb.CharacterId, cb.BackgroundId });

        builder.HasOne(cb => cb.Character)
            .WithMany(c => c.CharacterBackgrounds)
            .HasForeignKey(cb => cb.CharacterId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(cb => cb.Background)
            .WithMany()
            .HasForeignKey(cb => cb.BackgroundId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(cb => cb.Level)
            .IsRequired()
            .HasMaxLength(100);
    }
}
