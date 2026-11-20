using Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Rooms;

internal class RoomMapTokenConfiguration : IEntityTypeConfiguration<RoomMapToken>
{
    public void Configure(EntityTypeBuilder<RoomMapToken> builder)
    {
        builder.ToTable("RoomMapTokens");
        builder.HasKey(token => token.Id);

        builder.Property(token => token.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.HasIndex(token => new { token.IdRoomMap, token.MemberId, token.IsNpc })
            .IsUnique();

        builder.HasOne(token => token.RoomMap)
            .WithMany(map => map.Tokens)
            .HasForeignKey(token => token.IdRoomMap)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
