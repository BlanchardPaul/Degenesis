using Domain.Burns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityConfiguration.Burns;
internal class NPCBurnConfiguration : IEntityTypeConfiguration<NPCBurn>
{
    public void Configure(EntityTypeBuilder<NPCBurn> builder)
    {
        builder.ToTable("NPCBurns");

        builder.HasKey(nb => new { nb.NPCId, nb.BurnId });

        builder.Property(nb => nb.Quantity)
            .IsRequired();

        builder.HasOne(nb => nb.NPC)
            .WithMany(n => n.NPCBurns)
            .HasForeignKey(nb => nb.NPCId);

        builder.HasOne(nb => nb.Burn)
            .WithMany()
            .HasForeignKey(nb => nb.BurnId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}