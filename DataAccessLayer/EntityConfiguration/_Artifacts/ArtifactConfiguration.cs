using Domain._Artifacts;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration._Artifacts;
internal class ArtifactConfiguration : IEntityTypeConfiguration<Artifact>
{
    public void Configure(EntityTypeBuilder<Artifact> builder)
    {
        builder.ToTable("Artifacts");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.EnergyStorage)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Magazine);
        builder.Property(p => p.Encumbrance);
        builder.Property(p => p.Activation);
        builder.Property(p => p.Value);
    }
}