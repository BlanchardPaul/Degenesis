using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Characters.Inventory;

namespace DataAccessLayer.EntityConfiguration.Characters.Inventory;
class CharacterBurnConfiguration : IEntityTypeConfiguration<CharacterBurn>
{
    public void Configure(EntityTypeBuilder<CharacterBurn> builder)
    {
        builder.ToTable("CharacterBurns");

        builder.HasKey(cb => cb.Id);

        builder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasOne(cb => cb.Character)
            .WithMany(c => c.CharacterBurns)
            .HasForeignKey(cb => cb.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cb => cb.Burn)
            .WithMany()
            .HasForeignKey(b => b.BurnId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(cb => cb.WeakQuantity);
        builder.Property(cb => cb.PotentQuantity);
    }
}