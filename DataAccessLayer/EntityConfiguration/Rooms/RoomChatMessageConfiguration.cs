using Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Rooms;

internal class RoomChatMessageConfiguration : IEntityTypeConfiguration<RoomChatMessage>
{
    public void Configure(EntityTypeBuilder<RoomChatMessage> builder)
    {
        builder.ToTable("RoomChatMessages");

        builder.HasKey(rcm => rcm.Id);

        builder.Property(m => m.Text).HasMaxLength(2000);

        builder.HasOne(rcm => rcm.Room)
            .WithMany()
            .HasForeignKey(rcm => rcm.IdRoom)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
