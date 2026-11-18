using Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Rooms;

internal class RoomMapConfiguration : IEntityTypeConfiguration<RoomMap>
{
    public void Configure(EntityTypeBuilder<RoomMap> builder)
    {
        builder.ToTable("RoomMaps");

        builder.HasKey(map => map.Id);

        builder.Property(map => map.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(map => map.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(map => map.FileName)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(map => map.Room)
            .WithMany(room => room.Maps)
            .HasForeignKey(map => map.IdRoom)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
