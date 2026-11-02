using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class BaseNPCAttributeConfiguration : IEntityTypeConfiguration<BaseNPCAttribute>
{
    public void Configure(EntityTypeBuilder<BaseNPCAttribute> builder)
    {
        builder.ToTable("BaseNPCAttributes");

        builder.HasKey(na => new { na.BaseNPCId, na.AttributeId });

        builder.Property(na => na.Level)
            .IsRequired();

        builder.HasOne(na => na.BaseNPC)
            .WithMany(n => n.BaseNPCAttributes)
            .HasForeignKey(na => na.BaseNPCId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(na => na.Attribute)
            .WithMany()
            .HasForeignKey(na => na.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}