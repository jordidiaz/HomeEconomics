namespace HomeEconomics.Features.MovementMonths
{
    public class MovementMonthResponse
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public MonthMovementResult[] MonthMovements { get; set; }

        public class MonthMovementResult
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public decimal Amount { get; set; }

            public int Type { get; set; }
        }
    }
}
