using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            .HasForeignKey(na => na.NPCId);

        builder.HasOne(na => na.Attribute)
            .WithMany()
            .HasForeignKey(na => na.AttributeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}