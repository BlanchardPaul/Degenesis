using Domain.Burns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Burns;
internal class BurnConfiguration : IEntityTypeConfiguration<Burn>
{
    public void Configure(EntityTypeBuilder<Burn> builder)
    {
        builder.ToTable("Burns");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasDefaultValueSql("NEWID()");

            builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(b => b.Chakra)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.EarthChakra)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Effect)
            .IsRequired();

        builder.Property(b => b.WeakCost)
            .IsRequired();

        builder.Property(b => b.PotentCost)
            .IsRequired();
    }
}