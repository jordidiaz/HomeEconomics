using Domain;
using Domain.MovementMonth;
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
                .HasMaxLength(Lengths.Name)
                .IsRequired();
        }
    }
}
