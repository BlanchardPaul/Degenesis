using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Rooms;

namespace DataAccessLayer.EntityConfiguration.Rooms;
internal class UserRoomConfiguration : IEntityTypeConfiguration<UserRoom>
{
    public void Configure(EntityTypeBuilder<UserRoom> builder)
    {
        builder.ToTable("UserRooms");

        builder.HasKey(ur => ur.Id);

        builder.Property(ur => ur.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(ur => ur.IsGM)
            .IsRequired();

        builder.Property(ur => ur.InvitationAccepted)
         .IsRequired();

        builder.HasOne(ur => ur.ApplicationUser)
            .WithMany(u => u.UserRooms)
            .HasForeignKey(ur => ur.IdApplicationUser);

        builder.HasOne(ur => ur.Room)
            .WithMany(r => r.UserRooms)
            .HasForeignKey(ur => ur.IdRoom)
            .OnDelete(DeleteBehavior.Cascade);
    }
}