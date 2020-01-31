using System;
using System.Collections.Generic;
using Domain.Movements;

namespace Domain.MovementMonth
{
    public class MovementMonth: Entity, IAggregateRoot
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
        }

        public int Year { get; private set; }

        public Month Month { get; private set; }

        public List<MonthMovement> MonthMovements { get; private set; }

        public void AddMonthMovement(string name, decimal amount, MovementType type)
        {
            var monthMovement = new MonthMovement(name, amount, type);
            MonthMovements.Add(monthMovement);
        }
    }
}
