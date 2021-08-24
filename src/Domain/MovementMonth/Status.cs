using System;
using Domain.Movements;

namespace Domain.MovementMonth
{
    public class Status : Entity
    {
#nullable disable
        protected Status()
        {

        }
#nullable enable

        internal Status(MovementMonth movementMonth, int day, decimal accountAmount, decimal cashAmount)
        {
            if (day < 0 || day > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            if (accountAmount < Movement.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(accountAmount));
            }

            if (cashAmount < Movement.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(cashAmount));
            }

            Day = day;
            AccountAmount = accountAmount;
            CashAmount = cashAmount;
            MovementMonth = movementMonth;
            MovementMonthId = movementMonth.Id;
        }

        public int Day { get; private set; }

        public decimal AccountAmount { get; private set; }

        public decimal CashAmount { get; private set; }

        public int MovementMonthId { get; private set; }

        public MovementMonth MovementMonth { get; private set; }


        internal void SetAccountAmount(decimal accountAmount)
        {
            if (accountAmount < Movement.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(accountAmount));
            }

            AccountAmount = accountAmount;
        }

        internal void SetCashAmount(decimal cashAmount)
        {
            if (cashAmount < Movement.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(cashAmount));
            }

            CashAmount = cashAmount;
        }
    }
}
