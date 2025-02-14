using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters;

namespace DataAccessLayer.EntityConfiguration.Characters;

internal sealed class ConceptConfiguration : IEntityTypeConfiguration<Concept>
{
    public void Configure(EntityTypeBuilder<Concept> builder)
    {
        builder.ToTable("Concepts");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasDefaultValueSql("NEWID()")
            .HasColumnOrder(1);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(c => c.BonusAttribute)
            .WithMany()
            .HasForeignKey(c => c.BonusAttributeId)
            .IsRequired();

        builder.HasMany(c => c.BonusSkills)
            .WithMany()
            .UsingEntity<ConceptSkill>(
                j => j.HasOne(cs => cs.Skill).WithMany().HasForeignKey(cs => cs.SkillId).OnDelete(DeleteBehavior.NoAction),
                j => j.HasOne(cs => cs.Concept).WithMany().HasForeignKey(cs => cs.ConceptId).OnDelete(DeleteBehavior.NoAction)
    );
    }
}
