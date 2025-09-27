using Domain.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Characters;
internal sealed class PotentialPrerequisiteConfiguration : IEntityTypeConfiguration<PotentialPrerequisite>
{
    public void Configure(EntityTypeBuilder<PotentialPrerequisite> builder)
    {
        builder.ToTable("PotentialPrerequisites");

        builder.HasKey(pp => pp.Id);

        builder.Property(pp => pp.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(pp => pp.SumRequired);

        builder.Property(pp => pp.BackgroundLevelRequired);

        builder.Property(pp => pp.IsBackgroundPrerequisite)
            .IsRequired();

        builder.Property(pp => pp.IsRankPrerequisite)
            .IsRequired();

        builder.HasOne(pp => pp.AttributeRequired)
            .WithMany()
            .HasForeignKey(pp => pp.AttributeRequiredId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pp => pp.SkillRequired)
            .WithMany()
            .HasForeignKey(pp => pp.SkillRequiredId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(pp => pp.BackgroundRequired)
            .WithMany()
            .HasForeignKey(pp => pp.BackgroundRequiredId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pp => pp.RankRequired)
            .WithMany()
            .HasForeignKey(pp => pp.RankRequiredId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}