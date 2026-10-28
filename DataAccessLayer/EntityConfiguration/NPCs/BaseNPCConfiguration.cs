using Domain.NPCs;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.EntityConfiguration.NPCs;
internal class BaseNPCConfiguration : IEntityTypeConfiguration<BaseNPC>
{
    public void Configure(EntityTypeBuilder<BaseNPC> builder)
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
    }
}