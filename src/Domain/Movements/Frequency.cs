using System;
using System.Linq;

namespace Domain.Movements
{
    public class Frequency : Entity
    {
#nullable disable
        public Frequency()
        {
            Months = MonthCollection.Init();
        }
#nullable enable

        internal Frequency(Movement movement, FrequencyType type)
        {
            Type = type;
            Movement = movement;
            MovementId = movement.Id;
            Months = MonthCollection.Init();
        }

        public FrequencyType Type { get; private set; }

        public MonthCollection Months { get; private set; }

        public int MovementId { get; private set; }

        public Movement Movement { get; private set; }

        internal void SetNoneFrequency()
        {
            Type = FrequencyType.None;
        }

        internal void SetMonthlyFrequency()
        {
            Type = FrequencyType.Monthly;
        }

        internal void SetYearlyFrequency(int month)
        {
            if (month is < 1 or > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month));
            }

            Type = FrequencyType.Yearly;
            Months.EnableMonth(month);
        }

        internal void SetCustomFrequency(bool[] months)
        {
            if (months.Length != 12)
            {
                throw new ArgumentOutOfRangeException(nameof(months));
            }

            if (months.All(month => !month))
            {
                throw new InvalidOperationException(Properties.Messages.NoMonthSelected);
            }

            Type = FrequencyType.Custom;
            Months = MonthCollection.Init(months);
        }
        
        internal bool IsMonthEnabled(int month)
        {
            return Months.IsMonthEnabled(month);
        }
    }
}
