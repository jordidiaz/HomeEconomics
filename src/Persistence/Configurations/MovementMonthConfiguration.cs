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

            builder.HasMany("_monthMovements")
                .WithOne()
                .HasForeignKey(nameof(MonthMovement.MovementMonthId))
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany("_statuses")
                .WithOne()
                .HasForeignKey(nameof(Status.MovementMonthId))
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
