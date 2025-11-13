namespace Domain.Movements;

public sealed class Frequency : Entity
{
    public Frequency()
    {
        Months = MonthCollection.Init();
    }

    internal Frequency(Movement movement, FrequencyType type)
    {
        Type = type;
        MovementId = movement.Id;
        Months = MonthCollection.Init();
    }

    public FrequencyType Type { get; private set; }
    public MonthCollection Months { get; private set; }
    public int MovementId { get; private set; }

    internal void SetNoneFrequency() => Type = FrequencyType.None;
    internal void SetMonthlyFrequency() => Type = FrequencyType.Monthly;

    internal void SetYearlyFrequency(int month)
    {
        if (month is < 1 or > 12)
        {
            throw new ArgumentOutOfRangeException(nameof(month));
        }
        Type = FrequencyType.Yearly;
        Months.EnableMonth(month);
    }

    internal void SetCustomFrequency(bool[] months)
    {
        if (months.Length != 12) throw new ArgumentOutOfRangeException(nameof(months));
        if (months.All(m => m is false))
        {
            throw new InvalidOperationException(Properties.Messages.NoMonthSelected);
        }
        Type = FrequencyType.Custom;
        Months = MonthCollection.Init(months);
    }

    internal bool IsMonthEnabled(int month) => Months.IsMonthEnabled(month);
    internal bool[] GetMonths() => Months.GetMonths();
}