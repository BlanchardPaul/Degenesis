using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class BaseNPCSkillConfiguration : IEntityTypeConfiguration<BaseNPCSkill>
{
    public void Configure(EntityTypeBuilder<BaseNPCSkill> builder)
    {
        builder.ToTable("BaseNPCSkills");

        builder.HasKey(ns => new { ns.BaseNPCId, ns.SkillId });

        builder.Property(ns => ns.Level)
            .IsRequired();

        builder.HasOne(ns => ns.BaseNPC)
            .WithMany(n => n.BaseNPCSkills)
            .HasForeignKey(ns => ns.BaseNPCId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ns => ns.Skill)
            .WithMany()
            .HasForeignKey(ns => ns.SkillId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}