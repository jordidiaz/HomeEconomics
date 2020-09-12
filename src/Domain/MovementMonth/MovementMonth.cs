using Domain.Movements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MovementMonth
{
    public class MovementMonth : Entity, IAggregateRoot
    {
        public MovementMonth(int year, Month month)
        {
            var currentYear = DateTime.Now.Year;

            if (year < currentYear)
            {
                throw new ArgumentOutOfRangeException(nameof(year));
            }

            Year = year;
            Month = month;
            MonthMovements = new List<MonthMovement>();
            Statuses = new List<Status>();
        }

        public int Year { get; private set; }

        public Month Month { get; private set; }

        public List<MonthMovement> MonthMovements { get; private set; }

        public List<Status> Statuses { get; private set; }


        public void AddMonthMovement(string name, decimal amount, MovementType type)
        {
            var monthMovement = new MonthMovement(name, amount, type);
            MonthMovements.Add(monthMovement);
        }

        public void PayMonthMovement(int monthMovementId)
        {
            var monthMovement = GetMonthMovementOrThrow(monthMovementId);
            monthMovement.Pay();
        }

        public void UnPayMonthMovement(int monthMovementId)
        {
            var monthMovement = GetMonthMovementOrThrow(monthMovementId);
            monthMovement.UnPay();
        }

        public void UpdateMonthMovementAmount(int monthMovementId, decimal amount)
        {
            var monthMovement = GetMonthMovementOrThrow(monthMovementId);
            monthMovement.SetAmount(amount);
        }

        public void DeleteMonthMovement(int monthMovementId)
        {
            var monthMovement = GetMonthMovementOrThrow(monthMovementId);
            MonthMovements.Remove(monthMovement);
        }

        public void AddStatus(int day, decimal accountAmount, decimal cashAmount)
        {
            if (day < 0 || day > 31)
            {
                throw new ArgumentOutOfRangeException(nameof(day));
            }

            if (accountAmount < Constants.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(accountAmount));
            }

            if (cashAmount < Constants.MinAmount)
            {
                throw new ArgumentOutOfRangeException(nameof(cashAmount));
            }

            var status = Statuses.SingleOrDefault(s => s.Day == day);
            if (status is null)
            {
                Statuses.Add(new Status(day, accountAmount, cashAmount));
            }
            else
            {
                status.SetAccountAmount(accountAmount);
                status.SetCashAmount(cashAmount);
            }
        }

        public MonthMovement GetMonthMovement(int monthMovementId)
        {
            return MonthMovements.SingleOrDefault(mm => mm.Id == monthMovementId);
        }

        private MonthMovement GetMonthMovementOrThrow(int monthMovementId)
        {
            var monthMovement = GetMonthMovement(monthMovementId);
            if (monthMovement is null)
            {
                throw new InvalidOperationException(Properties.Messages.MonthMovementNotExists);
            }

            return monthMovement;
        }
    }
}
