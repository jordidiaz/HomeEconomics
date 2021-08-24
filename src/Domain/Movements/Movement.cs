using Domain.MovementMonth;
using System;

namespace Domain.Movements
{
    public class Movement : Entity, IAggregateRoot
    {
        public const int MovementNameMaxLength = 30;
        public const decimal MinAmount = 0;
        
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
            Frequency = new Frequency(this, FrequencyType.None);
        }

        public string Name { get; private set; }

        public decimal Amount { get; private set; }

        public MovementType Type { get; set; }

        public Frequency Frequency { get; private set; }

        public void SetNoneFrequency()
        {
            Frequency.SetNoneFrequency();
        }

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

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        public void SetAmount(decimal amount)
        {
            if (amount < MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Amount = amount;
        }

        public FrequencyType GetFrequencyType()
        {
            return Frequency.Type;
        }

        public bool HasMonthInFrequency(Month month)
        {
            if (GetFrequencyType() == FrequencyType.None || GetFrequencyType() == FrequencyType.Monthly)
            {
                return false;
            }

            var monthNumber = (int)month;

            return Frequency.IsMonthEnabled(monthNumber);
        }
    }
}
