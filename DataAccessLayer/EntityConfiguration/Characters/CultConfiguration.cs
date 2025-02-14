using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;
internal class CultConfiguration : IEntityTypeConfiguration<Cult>
{
    public void Configure(EntityTypeBuilder<Cult> builder)
    {
        builder.ToTable("Cults");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasMany(c => c.BonusSkills)
            .WithMany();
    }
}