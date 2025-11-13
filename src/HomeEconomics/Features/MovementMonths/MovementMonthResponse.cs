using Domain.MovementMonth;
using Domain.Movements;

namespace HomeEconomics.Features.MovementMonths;

public record MovementMonthResponse
{
    public int Id { get; init; }

    public int Year { get; init; }

    public int Month { get; init; }

    public bool NextMovementMonthExists { get; set; }

    public StatusResult Status { get; init; } = new();

    public MonthMovementResult[] MonthMovements { get; set; } = { };

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
        
    public static MovementMonthResponse FromMovementMonth(MovementMonth movementMonth)
    {
        var statuses = movementMonth.GetStatuses().ToArray();
        var status = statuses.Any() ? statuses.OrderByDescending(status => status.Day).First() : null;
        var accountAmount = status?.AccountAmount ?? 0;
        var cashAmount = status?.CashAmount ?? 0;

        var monthMovements = movementMonth.GetMonthMovements().ToArray();
        var pendingTotalExpenses = GetPendingTotal(monthMovements, MovementType.Expense);
        var pendingTotalIncomes = GetPendingTotal(monthMovements, MovementType.Income);

        return new MovementMonthResponse
        {
            Id = movementMonth.Id,
            Year = movementMonth.Year,
            Month = (int)movementMonth.Month,
            Status = new StatusResult
            {
                AccountAmount = accountAmount,
                CashAmount = cashAmount,
                PendingTotalExpenses = pendingTotalExpenses,
                PendingTotalIncomes = pendingTotalIncomes,
            },
            MonthMovements = monthMovements
                .Select(FromMonthMovement)
                .OrderBy(mm => mm.Name).ToArray()
        };
    }

    private static decimal GetPendingTotal(MonthMovement[] monthMovements, MovementType movementType) => monthMovements.Where(mm => mm.Type == movementType && !mm.Paid).Sum(mm => mm.Amount);

    private static MonthMovementResult FromMonthMovement(MonthMovement monthMovement) =>
        new()
        {
            Id = monthMovement.Id,
            Name = monthMovement.Name,
            Amount = monthMovement.Amount,
            Type = (int)monthMovement.Type,
            Paid = monthMovement.Paid
        };
}