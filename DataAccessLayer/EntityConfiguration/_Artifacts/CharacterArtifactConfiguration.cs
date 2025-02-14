using Domain.Artifacts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityConfiguration.Artifacts;
internal class CharacterArtifactConfiguration : IEntityTypeConfiguration<CharacterArtifact>
{
    public void Configure(EntityTypeBuilder<CharacterArtifact> builder)
    {
        builder.ToTable("CharacterArtifacts");

        builder.HasKey(cp => new { cp.CharacterId, cp.ArtifactId });

        builder.HasOne(cp => cp.Character)
            .WithMany(c => c.CharacterArtifacts)
            .HasForeignKey(cp => cp.CharacterId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(cp => cp.Artifact)
            .WithMany()
            .IsRequired()
            .HasForeignKey(c => c.ArtifactId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(cp => cp.ChargeInMagazine);
    }
}