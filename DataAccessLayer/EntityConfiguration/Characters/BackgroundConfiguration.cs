using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;
internal class BackgroundConfiguration : IEntityTypeConfiguration<Background>
{
    public void Configure(EntityTypeBuilder<Background> builder)
    {
        builder.ToTable("Backgrounds");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever()
            .HasDefaultValueSql("NEWID()");

        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(100);
    }
}