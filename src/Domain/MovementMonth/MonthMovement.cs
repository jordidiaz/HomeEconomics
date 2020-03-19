using System;
using Domain.Movements;

namespace Domain.MovementMonth
{
    public class MonthMovement : Entity
    {
        internal MonthMovement(string name, decimal amount, MovementType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (amount < Constants.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Amount = amount;
            Type = type;
            Name = name;
        }

        public string Name { get; private set; }

        public decimal Amount { get; private set; }

        public MovementType Type { get; private set; }

        public bool Paid { get; private set; }

        public int MovementMonthId { get; private set; }

        public MovementMonth MovementMonth { get; private set; }


        internal void Pay()
        {
            Paid = true;
        }

        internal void UnPay()
        {
            Paid = false;
        }

        internal void SetAmount(decimal amount)
        {
            if (amount < Constants.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Amount = amount;
        }
    }
}
