using Domain.Movements;

namespace Domain.MovementMonth;

public class MovementMonth : Entity, IAggregateRoot
{
    public MovementMonth(int year, Month month)
    {
        var currentYear = DateTime.Now.Year;

        ArgumentOutOfRangeException.ThrowIfLessThan(year, currentYear);

        Year = year;
        Month = month;
        _monthMovements = MonthMovementCollection.Init();
        _statuses = StatusCollection.Init();
    }

    public int Year { get; private set; }

    public Month Month { get; private set; }

    private readonly MonthMovementCollection _monthMovements;
        
    private readonly StatusCollection _statuses;

    public void AddMonthMovement(string name, decimal amount, MovementType type)
    {
        var monthMovement = new MonthMovement(this, name, amount, type);
        _monthMovements.Add(monthMovement);
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
        _monthMovements.Remove(monthMovement);
    }

    public void AddStatus(int day, decimal accountAmount, decimal cashAmount)
    {
        var status = _statuses.GetByDay(day);
        if (status is null)
        {
            _statuses.Add(new Status(this, day, accountAmount, cashAmount));
        }
        else
        {
            status.SetAccountAmount(accountAmount);
            status.SetCashAmount(cashAmount);
        }
    }

    public MonthMovement? GetMonthMovement(int monthMovementId) => _monthMovements.Get(monthMovementId);

    public void ClearMovementMonths() => _monthMovements.Clear();

    public IEnumerable<MonthMovement> GetMonthMovements()
        => _monthMovements;
        
    public IEnumerable<Status> GetStatuses()
        => _statuses;

    private MonthMovement GetMonthMovementOrThrow(int monthMovementId)
    {
        var monthMovement = GetMonthMovement(monthMovementId);
        return monthMovement ?? throw new InvalidOperationException(Properties.Messages.MonthMovementNotExists);
    }
}
