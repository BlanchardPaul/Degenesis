using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class NPCPotentialConfiguration : IEntityTypeConfiguration<NPCPotential>
{
    public void Configure(EntityTypeBuilder<NPCPotential> builder)
    {
        builder.ToTable("NPCPotentials");

        builder.HasKey(np => new { np.NPCId, np.PotentialId });

        builder.Property(np => np.Level)
            .IsRequired();

        builder.HasOne(np => np.NPC)
            .WithMany(n => n.NPCPotentials)
            .HasForeignKey(np => np.NPCId);

        builder.HasOne(np => np.Potential)
            .WithMany()
            .HasForeignKey(np => np.PotentialId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}