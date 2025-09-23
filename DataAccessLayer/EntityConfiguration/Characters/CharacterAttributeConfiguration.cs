using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;

internal sealed class CharacterAttributeConfiguration : IEntityTypeConfiguration<CharacterAttribute>
{
    public void Configure(EntityTypeBuilder<CharacterAttribute> builder)
    {
        builder.ToTable("CharacterAttributes");

        builder.HasKey(ca => new { ca.CharacterId, ca.AttributeId });

        builder.HasOne(ca => ca.Character)
            .WithMany(c => c.CharacterAttributes)
            .HasForeignKey(c => c.CharacterId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.HasOne(ca => ca.Attribute)
            .WithMany()
             .IsRequired()
            .HasForeignKey(c => c.AttributeId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(ca => ca.Level)
            .IsRequired();
    }
}