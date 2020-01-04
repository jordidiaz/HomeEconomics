using System;
using System.Linq;
using Domain.Movements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Configurations
{
    internal class FrequencyConfiguration : IEntityTypeConfiguration<Frequency>
    {
        public void Configure(EntityTypeBuilder<Frequency> builder)
        {
            var converter = new ValueConverter<bool[], string>(
                months => string.Join(",", months.Select(x => x ? 1 : 0)),
                months => months.Split(',', StringSplitOptions.None).Select(x => x == "1").ToArray()
                );

            builder.ToTable("Frequencies");
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Months)
                .HasMaxLength(23)
                .HasConversion(converter);
        }
    }
}
