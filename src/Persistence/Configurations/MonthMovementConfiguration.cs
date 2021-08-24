using Domain.MovementMonth;
using Domain.Movements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal class MonthMovementConfiguration : IEntityTypeConfiguration<MonthMovement>
    {
        public void Configure(EntityTypeBuilder<MonthMovement> builder)
        {
            builder.ToTable("MonthMovements");

            builder.HasKey(monthMovement => monthMovement.Id);

            builder.Property(monthMovement => monthMovement.Name)
                .HasMaxLength(Movement.MovementNameMaxLength)
                .IsRequired();

            builder.Property(monthMovement => monthMovement.Amount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
