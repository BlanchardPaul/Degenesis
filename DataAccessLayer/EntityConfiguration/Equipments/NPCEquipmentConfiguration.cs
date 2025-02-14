using Domain.Equipments;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityConfiguration.Equipments;
internal class NPCEquipmentConfiguration : IEntityTypeConfiguration<NPCEquipment>
{
    public void Configure(EntityTypeBuilder<NPCEquipment> builder)
    {
        builder.ToTable("NPCEquipments");

        builder.HasKey(ne => ne.Id);

        builder.Property(ne => ne.UsedSlots)
            .IsRequired();

        builder.HasOne(ne => ne.NPC)
            .WithMany(n => n.NPCEquipments)
            .HasForeignKey(ne => ne.NPCId);

        builder.HasOne(ne => ne.Equipment)
            .WithMany()
            .HasForeignKey(ne => ne.EquipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}