using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class NPCConfiguration : IEntityTypeConfiguration<NPC>
{
    public void Configure(EntityTypeBuilder<NPC> builder)
    {
        builder.ToTable("NPCs");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(n => n.Height)
            .IsRequired();

        builder.Property(n => n.Weight)
            .IsRequired();

        builder.Property(n => n.Ego)
            .IsRequired();

        builder.Property(n => n.FleshWounds)
            .IsRequired();

        builder.Property(n => n.Trauma)
            .IsRequired();

        builder.Property(n => n.PassiveDefense)
            .IsRequired();

        builder.Property(n => n.EnemySpec)
            .IsRequired();
    }
}