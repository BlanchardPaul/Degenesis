using Domain.Burns;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Burns;
class CharacterBurnConfiguration : IEntityTypeConfiguration<CharacterBurn>
{
    public void Configure(EntityTypeBuilder<CharacterBurn> builder)
    {
        builder.ToTable("CharacterBurns");

        builder.HasKey(cb => new { cb.CharacterId, cb.BurnId });

        builder.Property(cb => cb.Quantity)
            .IsRequired();

        builder.HasOne(cb => cb.Character)
            .WithMany(c => c.CharacterBurns)
            .HasForeignKey(cb => cb.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cb => cb.Burn)
            .WithMany()
            .HasForeignKey(cb => cb.BurnId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}