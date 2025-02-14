using Domain.Weapons;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Weapons;
internal class WeaponTypeConfiguration : IEntityTypeConfiguration<WeaponType>
{
    public void Configure(EntityTypeBuilder<WeaponType> builder)
    {
        builder.ToTable("WeaponTypes");

        builder.HasKey(wt => wt.Id);

        builder.Property(wt => wt.Id)
            .IsRequired()
            .HasDefaultValueSql("NEWID()");

        builder.Property(wt => wt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(wt => wt.Description)
            .IsRequired()
            .HasMaxLength(1000);
    }
}