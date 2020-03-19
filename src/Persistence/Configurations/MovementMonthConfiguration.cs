using Domain.MovementMonth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal class MovementMonthConfiguration : IEntityTypeConfiguration<MovementMonth>
    {
        public void Configure(EntityTypeBuilder<MovementMonth> builder)
        {
            builder.ToTable("MovementMonths");
            builder.HasKey(movementMonth => movementMonth.Id);
            builder.HasIndex(movementMonth => new { movementMonth.Year, movementMonth.Month })
                .IsUnique();
            builder.HasMany(movementMonth => movementMonth.MonthMovements)
                .WithOne(monthMovement => monthMovement.MovementMonth)
                .HasForeignKey(monthMovement => monthMovement.MovementMonthId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(movementMonth => movementMonth.Statuses)
                .WithOne(status => status.MovementMonth)
                .HasForeignKey(monthMovement => monthMovement.MovementMonthId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
