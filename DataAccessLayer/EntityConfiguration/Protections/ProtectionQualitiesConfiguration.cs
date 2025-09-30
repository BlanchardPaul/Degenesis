using Domain.Protections;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Protections;
internal class ProtectionQualitiesConfiguration : IEntityTypeConfiguration<ProtectionQuality>
{
    public void Configure(EntityTypeBuilder<ProtectionQuality> builder)
    {
        builder.ToTable("ProtectionQualities");

        builder.HasKey(pq => pq.Id);

        builder.Property(pq => pq.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pq => pq.Description)
            .IsRequired();
    }
}