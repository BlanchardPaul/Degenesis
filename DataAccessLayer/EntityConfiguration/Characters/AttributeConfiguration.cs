using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters;

namespace DataAccessLayer.EntityConfiguration.Characters;
internal class AttributeConfiguration : IEntityTypeConfiguration<CAttribute>
{
    public void Configure(EntityTypeBuilder<CAttribute> builder)
    {
        // Nom de la table
        builder.ToTable("Attributes");

        // Clé primaire avec valeur par défaut (équivalent de DEFAULT NEWID())
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
               .HasDefaultValueSql("NEWID()");

        // Contraintes de colonnes
        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.Abbreviation)
               .IsRequired()
               .HasMaxLength(10);

        builder.Property(a => a.Description)
               .IsRequired()
               .HasMaxLength(1000);
    }
}