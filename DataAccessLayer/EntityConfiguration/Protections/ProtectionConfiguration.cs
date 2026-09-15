using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Protections;

namespace DataAccessLayer.EntityConfiguration.Protections;

internal sealed class ProtectionConfiguration : IEntityTypeConfiguration<Protection>
{
    public void Configure(EntityTypeBuilder<Protection> builder)
    {
        builder.ToTable("Protections");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasDefaultValueSql("NEWID()")
            .HasColumnOrder(1);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired();

        builder.Property(p => p.Armor);
        builder.Property(p => p.Slots);

        builder.Property(p => p.Defense)
            .HasMaxLength(1000);

        builder.Property(p => p.Attack)
            .HasMaxLength(1000);

        builder.Property(p => p.Encumbrance);
        builder.Property(p => p.TechLevel);
        builder.Property(p => p.Value);
        builder.Property(p => p.Resources);
        builder.Property(p => p.Qualities);

        builder.HasMany(p => p.Cults)
            .WithMany();

    }
}