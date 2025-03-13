using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;
internal class RankPrerequisitesConfiguration : IEntityTypeConfiguration<RankPrerequisite>
{
    public void Configure(EntityTypeBuilder<RankPrerequisite> builder)
    {
        builder.ToTable("RankPrerequisites");

        builder.HasKey(rp => rp.Id);

        builder.Property(rp => rp.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(rp => rp.SumRequired)
            .IsRequired();

        builder.HasOne(rp => rp.AttributeRequired)
            .WithMany()
            .HasForeignKey(rp => rp.AttributeRequiredId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.SkillRequired)
            .WithMany()
            .HasForeignKey(rp => rp.SkillRequiredId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}