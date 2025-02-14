using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class NPCSkillConfiguration : IEntityTypeConfiguration<NPCSkill>
{
    public void Configure(EntityTypeBuilder<NPCSkill> builder)
    {
        builder.ToTable("NPCSkills");

        builder.HasKey(ns => new { ns.NPCId, ns.SkillId });

        builder.Property(ns => ns.Level)
            .IsRequired();

        builder.HasOne(ns => ns.NPC)
            .WithMany(n => n.NPCSkills)
            .HasForeignKey(ns => ns.NPCId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ns => ns.Skill)
            .WithMany()
            .HasForeignKey(ns => ns.SkillId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}