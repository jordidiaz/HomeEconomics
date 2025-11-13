using Domain.MovementMonth;

namespace Domain.Movements;

public sealed class Movement : Entity, IAggregateRoot
{
    public const int MovementNameMaxLength = 30;
    public const decimal MinAmount = 0;

    public Movement(string name, decimal amount, MovementType type)
    {
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
        Amount = amount < MinAmount ? throw new ArgumentOutOfRangeException(nameof(amount)) : amount;
        Type = type;
        Frequency = new Frequency(this, FrequencyType.None);
    }

    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public MovementType Type { get; set; }
    public Frequency Frequency { get; private set; }

    public void SetNoneFrequency() => Frequency.SetNoneFrequency();
    public void SetMonthlyFrequency() => Frequency.SetMonthlyFrequency();
    public void SetYearlyFrequency(int month) => Frequency.SetYearlyFrequency(month);
    public void SetCustomFrequency(bool[] months) => Frequency.SetCustomFrequency(months);

    public void SetName(string name) => Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
    public void SetAmount(decimal amount) => Amount = amount < MinAmount ? throw new ArgumentOutOfRangeException(nameof(amount)) : amount;

    public FrequencyType GetFrequencyType() => Frequency.Type;

    public bool HasMonthInFrequency(Month month)
    {
        return GetFrequencyType() switch
        {
            FrequencyType.None => false,
            FrequencyType.Monthly => false,
            _ => Frequency.IsMonthEnabled((int)month)
        };
    }

    public bool[] GetMonths() => Frequency.GetMonths();
}