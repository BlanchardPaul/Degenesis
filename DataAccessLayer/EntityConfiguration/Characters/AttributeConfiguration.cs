using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters;

namespace DataAccessLayer.EntityConfiguration.Characters;
internal class AttributeConfiguration : IEntityTypeConfiguration<CAttribute>
{
    public void Configure(EntityTypeBuilder<CAttribute> builder)
    {
        builder.ToTable("Attributes");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
               .HasDefaultValueSql("NEWID()");

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.Abbreviation)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(a => a.Description)
               .IsRequired();
    }
}