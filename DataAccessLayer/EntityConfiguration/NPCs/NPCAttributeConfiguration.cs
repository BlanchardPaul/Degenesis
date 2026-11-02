using Domain.NPCs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class NPCAttributeConfiguration : IEntityTypeConfiguration<NPCAttribute>
{
    public void Configure(EntityTypeBuilder<NPCAttribute> builder)
    {
        builder.ToTable("NPCAttributes");

        builder.HasKey(na => new { na.NPCId, na.AttributeId });

        builder.Property(na => na.Level)
            .IsRequired();

        builder.HasOne(na => na.NPC)
            .WithMany(n => n.NPCAttributes)
            .HasForeignKey(na => na.NPCId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(na => na.Attribute)
            .WithMany()
            .HasForeignKey(na => na.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
