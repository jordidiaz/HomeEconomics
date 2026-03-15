namespace HomeEconomics.Features.MovementMonths;

public record MovementMonthResponse
{
    public int Id { get; init; }

    public int Year { get; init; }

    public int Month { get; init; }

    public bool NextMovementMonthExists { get; set; }

    public bool PreviousMovementMonthExists { get; set; }

    public StatusResult Status { get; init; } = new();

    public MonthMovementResult[] MonthMovements { get; set; } = [];

    public class MonthMovementResult
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public decimal Amount { get; init; }

        public int Type { get; init; }

        public bool Paid { get; init; }
    }

    public class StatusResult
    {
        public decimal PendingTotalExpenses { get; init; }

        public decimal PendingTotalIncomes { get; init; }

        public decimal AccountAmount { get; init; }

        public decimal CashAmount { get; init; }
    }
}
