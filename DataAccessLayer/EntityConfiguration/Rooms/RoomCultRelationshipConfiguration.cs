using Domain.Rooms;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.EntityConfiguration.Rooms;
internal class RoomCultRelationshipConfiguration : IEntityTypeConfiguration<RoomCultRelationship>
{
    public void Configure(EntityTypeBuilder<RoomCultRelationship> builder)
    {
        builder.ToTable("RoomCultRelationship");
        builder.HasKey(rcr => rcr.Id);

        builder.Property(rcr => rcr.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(rcr => rcr.IsDiscovered);
        builder.Property(rcr => rcr.Relationship);

        builder.HasOne(rcr => rcr.Cult)
            .WithMany()
            .HasForeignKey(rcr => rcr.IdCult)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rcr => rcr.Room)
            .WithMany()
            .HasForeignKey(rcr => rcr.IdRoom)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
