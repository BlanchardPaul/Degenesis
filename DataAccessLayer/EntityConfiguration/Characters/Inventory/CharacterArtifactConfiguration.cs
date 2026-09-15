using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters.Inventory;

namespace DataAccessLayer.EntityConfiguration.Characters.Inventory;
internal class CharacterArtifactConfiguration : IEntityTypeConfiguration<CharacterArtifact>
{
    public void Configure(EntityTypeBuilder<CharacterArtifact> builder)
    {
        builder.ToTable("CharacterArtifacts");

        builder.HasKey(ca => ca.Id);

        builder.Property(ca => ca.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(ca => ca.Character)
            .WithMany(c => c.CharacterArtifacts)
            .HasForeignKey(ca => ca.CharacterId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(ca => ca.Artifact)
            .WithMany()
            .IsRequired()
            .HasForeignKey(a => a.ArtifactId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.Property(ca => ca.ChargeInMagazine);
    }
}