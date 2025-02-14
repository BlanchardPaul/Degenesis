using Domain.Protections;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.Protections;
internal class NPCProtectionConfiguration : IEntityTypeConfiguration<NPCProtection>
{
    public void Configure(EntityTypeBuilder<NPCProtection> builder)
    {
        builder.ToTable("NPCProtections");

        builder.HasKey(np => np.Id);

        builder.Property(np => np.UsedConnectors)
            .IsRequired();

        builder.Property(np => np.UsedSlots)
            .IsRequired();

        builder.HasOne(np => np.NPC)
            .WithMany(n => n.NPCProtections)
            .HasForeignKey(np => np.NPCId);

        builder.HasOne(np => np.Protection)
            .WithMany()
            .HasForeignKey(np => np.ProtectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}