using System;
using System.Linq;

namespace Domain.Movements
{
    public class Frequency : Entity
    {
#nullable disable
        public Frequency()
        {

        }
#nullable enable

        internal Frequency(Movement movement, FrequencyType type)
        {
            Type = type;
            Months = new bool[12];
            Movement = movement;
            MovementId = movement.Id;
        }

        public FrequencyType Type { get; private set; }

        public bool[] Months { get; private set; }

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
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month));
            }

            Type = FrequencyType.Yearly;
            Months[month - 1] = true;
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
            Months = months;
        }
    }
}
