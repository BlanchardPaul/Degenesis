using Domain.Characters;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Characters;

internal sealed class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.ToTable("Characters");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasDefaultValueSql("NEWID()")
            .HasColumnOrder(1);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Age)
            .IsRequired();

        builder.Property(c => c.Height);

        builder.Property(c => c.Weight);

        builder.Property(c => c.Sex)
            .HasMaxLength(20);

        builder.Property(c => c.DinarMoney);

        builder.Property(c => c.ChroniclerMoney);

        builder.Property(c => c.MaxEgo);

        builder.Property(c => c.Ego);

        builder.Property(c => c.CurrentSporeInfestation);

        builder.Property(c => c.MaxSporeInfestation);

        builder.Property(c => c.PermanentSporeInfestation);

        builder.Property(c => c.MaxFleshWounds);

        builder.Property(c => c.FleshWounds);

        builder.Property(c => c.MaxTrauma);

        builder.Property(c => c.Trauma);

        builder.Property(c => c.PassiveDefense);

        builder.Property(c => c.Experience);

        builder.Property(c => c.Notes);

        builder.Property(c => c.InventoryNotes);

        builder.Property(a => a.IsFocusOriented);

        // For workflow
        builder.HasOne(c => c.Room)
            .WithMany(r => r.Characters)
            .HasForeignKey(c => c.IdRoom);

        builder.HasOne(c => c.ApplicationUser)
            .WithMany(u => u.Characters)
            .HasForeignKey(c => c.IdApplicationUser);

        // For Character stats
        builder.HasOne(c => c.Cult)
            .WithMany()
            .HasForeignKey(c => c.CultId)
            .IsRequired();

        builder.HasOne(c => c.Culture)
            .WithMany()
            .HasForeignKey(c => c.CultureId)
            .IsRequired();

        builder.HasOne(c => c.Concept)
            .WithMany()
            .HasForeignKey(c => c.ConceptId)
            .IsRequired();

        builder.HasOne(c => c.Rank)
           .WithMany()
           .HasForeignKey(c => c.RankId)
           .IsRequired()
           .OnDelete(DeleteBehavior.Restrict);

        //For Character Inventory
        builder.HasMany(c => c.CharacterWeapons)
            .WithOne(cw => cw.Character)
            .HasForeignKey(cw => cw.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CharacterEquipments)
            .WithOne(ce => ce.Character)
            .HasForeignKey(ce => ce.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CharacterProtections)
            .WithOne(cp => cp.Character)
            .HasForeignKey(cp => cp.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CharacterArtifacts)
            .WithOne(ca => ca.Character)
            .HasForeignKey(ca => ca.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CharacterVehicles)
            .WithOne(cv => cv.Character)
            .HasForeignKey(cv => cv.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CharacterBurns)
            .WithOne(cb => cb.Character)
            .HasForeignKey(cb => cb.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}