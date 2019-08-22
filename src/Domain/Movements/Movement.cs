using System;
using System.Linq;

namespace Domain.Movements
{
    public class Movement : Entity, IAggregateRoot
    {
        public const decimal MinAmount = 0.1m;

        public Movement(string name, decimal amount, MovementType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (amount < MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Name = name;
            Amount = amount;
            Type = type;
            Frequency = new Frequency(FrequencyType.None);
        }

        public string Name { get; private set; }

        public decimal Amount { get; private set; }

        public MovementType Type { get; private set; }

        public int FrequencyId { get; private set; }
    
        public Frequency Frequency { get; private set; }


        public void SetMonthlyFrequency()
        {
            Frequency.SetMonthlyFrequency();
        }

        public void SetYearlyFrequency(int month)
        {
            Frequency.SetYearlyFrequency(month);
        }

        public void SetCustomFrequency(bool[] months)
        {
            Frequency.SetCustomFrequency(months);
        }
    }
}
