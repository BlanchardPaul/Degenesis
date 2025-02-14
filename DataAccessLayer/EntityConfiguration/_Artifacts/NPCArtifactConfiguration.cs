using Domain._Artifacts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration._Artifacts;
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