using Domain.Movements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations
{
    internal class FrequencyConfiguration : IEntityTypeConfiguration<Frequency>
    {
        public void Configure(EntityTypeBuilder<Frequency> builder)
        {
            var converter = new ValueConverter<MonthCollection, string>(
                months => string.Join(",", months.Select(x => x ? 1 : 0)),
                months => MonthCollection.Init(months.Split(',', StringSplitOptions.None).Select(x => x == "1").ToList())
                );

            // https://github.com/dotnet/efcore/issues/17471
            var comparer = new ValueComparer<MonthCollection>(
                (months1, months2) => months1!.SequenceEqual(months2!),
                months => months.Aggregate(0, (i, b) => HashCode.Combine(i, b.GetHashCode())));

            builder.ToTable("Frequencies");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Months)
                .HasMaxLength(23)
                .HasConversion(converter)
                .Metadata.SetValueComparer(comparer);
        }
    }
}
