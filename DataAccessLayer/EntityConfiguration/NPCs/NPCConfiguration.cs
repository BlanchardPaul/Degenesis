using Domain.NPCs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class NPCConfiguration : IEntityTypeConfiguration<NPC>
{
    public void Configure(EntityTypeBuilder<NPC> builder)
    {
        builder.ToTable("NPCs");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Name)
            .IsRequired();

        builder.Property(n => n.Initiative);
        builder.Property(n => n.Specialty);
        builder.Property(n => n.Attack);
        builder.Property(n => n.Defense);
        builder.Property(n => n.Movement);
        builder.Property(n => n.Armor);

        builder.Property(n => n.Equipment);
        builder.Property(n => n.Condition);
        builder.Property(n => n.Notes);

        builder.HasOne(n => n.Room)
            .WithMany()
            .HasForeignKey(n => n.IdRoom)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
