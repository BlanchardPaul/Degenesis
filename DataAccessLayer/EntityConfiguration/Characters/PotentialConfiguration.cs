using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;

internal sealed class PotentialConfiguration : IEntityTypeConfiguration<Potential>
{
    public void Configure(EntityTypeBuilder<Potential> builder)
    {
        builder.ToTable("Potentials");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasDefaultValueSql("NEWID()")
            .HasColumnOrder(1);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne(p => p.Cult)
            .WithMany()
            .HasForeignKey(p => p.CultId);

        builder.HasMany(p => p.Prerequisites)
            .WithMany();
    }
}