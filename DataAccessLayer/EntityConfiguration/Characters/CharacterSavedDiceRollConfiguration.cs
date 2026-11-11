using Domain.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Characters;
internal class CharacterSavedDiceRollConfiguration : IEntityTypeConfiguration<CharacterSavedDiceRoll>
{
    public void Configure(EntityTypeBuilder<CharacterSavedDiceRoll> builder)
    {
        builder.ToTable("CharacterSavedDiceRolls");
        builder.HasKey(csdr => csdr.Id);

        builder.Property(csdr => csdr.Name)
            .HasMaxLength(100);

        builder.HasOne(csdr => csdr.Character)
            .WithMany(c => c.CharacterSavedDiceRolls)
            .HasForeignKey(csdr => csdr.CharacterId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(csdr => csdr.Attribute)
            .WithMany()
            .HasForeignKey(csdr => csdr.AttributeId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(csdr => csdr.Skill)
            .WithMany()
            .HasForeignKey(csdr => csdr.SkillId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}   