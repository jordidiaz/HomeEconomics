using Domain.Movements;

namespace Domain.MovementMonth;

public class MonthMovement : Entity
{
    // Parameterless constructor for EF
    protected MonthMovement() { }

    internal MonthMovement(MovementMonth movementMonth, string name, decimal amount, MovementType type)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
        Amount = amount < Movement.MinAmount ? throw new ArgumentOutOfRangeException(nameof(amount)) : amount;
        Type = type;
        MovementMonthId = movementMonth.Id;
    }

    public string Name { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public MovementType Type { get; private set; }
    public bool Paid { get; private set; }
    public int MovementMonthId { get; private set; }

    internal void Pay() => Paid = true;
    internal void UnPay() => Paid = false;

    internal void SetAmount(decimal amount) => Amount = amount < Movement.MinAmount ? throw new ArgumentOutOfRangeException(nameof(amount)) : amount;

    internal void SetName(string name) =>
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;

    internal void SetType(MovementType type) => Type = type;
}