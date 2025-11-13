using Domain.Movements;

namespace Domain.MovementMonth;

public class Status : Entity
{
    // ReSharper disable once UnusedMember.Global
    protected Status()
    {

    }

    internal Status(MovementMonth movementMonth, int day, decimal accountAmount, decimal cashAmount)
    {
        if (day is < 0 or > 31)
        {
            throw new ArgumentOutOfRangeException(nameof(day));
        }

        ArgumentOutOfRangeException.ThrowIfLessThan(accountAmount, Movement.MinAmount);

        ArgumentOutOfRangeException.ThrowIfLessThan(cashAmount, Movement.MinAmount);

        Day = day;
        AccountAmount = accountAmount;
        CashAmount = cashAmount;
        MovementMonthId = movementMonth.Id;
    }

    public int Day { get; private set; }

    public decimal AccountAmount { get; private set; }

    public decimal CashAmount { get; private set; }

    public int MovementMonthId { get; private set; }

    internal void SetAccountAmount(decimal accountAmount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(accountAmount, Movement.MinAmount);

        AccountAmount = accountAmount;
    }

    internal void SetCashAmount(decimal cashAmount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(cashAmount, Movement.MinAmount);

        CashAmount = cashAmount;
    }
}
