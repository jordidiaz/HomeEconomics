using Domain.Movements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal class MovementConfiguration : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.ToTable("Movements");

            builder.HasKey(m => m.Id);

            builder.HasIndex(m => m.Name)
                .IsUnique();

            builder.Property(m => m.Name)
                .HasMaxLength(Movement.MovementNameMaxLength)
                .IsRequired();

            builder.HasOne(movement => movement.Frequency)
                .WithOne(frequency => frequency.Movement)
                .HasForeignKey<Frequency>(frequency => frequency.MovementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(movement => movement.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
