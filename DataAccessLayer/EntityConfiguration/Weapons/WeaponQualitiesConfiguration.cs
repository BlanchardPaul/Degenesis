using Domain.Weapons;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Weapons;
internal class WeaponQualitiesConfiguration : IEntityTypeConfiguration<WeaponQuality>
{
    public void Configure(EntityTypeBuilder<WeaponQuality> builder)
    {
        builder.ToTable("WeaponQualities");

        builder.HasKey(wq => wq.Id);

        builder.Property(wq => wq.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(wq => wq.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(wq => wq.Description)
            .IsRequired();
    }
}