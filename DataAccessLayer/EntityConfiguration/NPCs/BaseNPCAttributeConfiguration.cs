using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class BaseNPCAttributeConfiguration : IEntityTypeConfiguration<BaseNPCAttribute>
{
    public void Configure(EntityTypeBuilder<BaseNPCAttribute> builder)
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