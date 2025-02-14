using Domain.Artifacts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityConfiguration.Artifacts;
internal class NPCArtifactConfiguration : IEntityTypeConfiguration<NPCArtifact>
{
    public void Configure(EntityTypeBuilder<NPCArtifact> builder)
    {
        builder.ToTable("NPCArtifacts");

        builder.HasKey(na => na.Id);

        builder.Property(na => na.Id)
               .HasDefaultValueSql("NEWID()");

        builder.HasOne(na => na.NPC)
                .WithMany(n => n.NPCArtifacts)
                .HasForeignKey(na => na.NPCId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(na => na.Artifact)
               .WithMany() 
               .HasForeignKey(na => na.ArtifactId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}