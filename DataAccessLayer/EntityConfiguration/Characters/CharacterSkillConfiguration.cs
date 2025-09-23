using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;

internal sealed class CharacterSkillConfiguration : IEntityTypeConfiguration<CharacterSkill>
{
    public void Configure(EntityTypeBuilder<CharacterSkill> builder)
    {
        builder.ToTable("CharacterSkills");
        builder.HasKey(cs => new { cs.CharacterId, cs.SkillId });

        builder.HasOne(cs => cs.Character)
            .WithMany(c => c.CharacterSkills)
            .HasForeignKey(c => c.CharacterId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(cs => cs.Skill)
            .WithMany()
            .HasForeignKey(cs => cs.SkillId)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();

        builder.Property(cs => cs.Level)
            .IsRequired();
    }
}