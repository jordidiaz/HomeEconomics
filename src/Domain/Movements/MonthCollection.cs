using System.Collections;

namespace Domain.Movements;

public class MonthCollection : ICollection<bool>
{
    private readonly IList<bool> _months;

    private MonthCollection(IList<bool>? months) => _months = months ?? Enumerable.Repeat(false, 12).ToList();

    public static MonthCollection Init(IList<bool> months) => new(months);

    public static MonthCollection Init() => new(null);

    internal void EnableMonth(int month) => _months[month - 1] = true;

    internal bool IsMonthEnabled(int month) => _months[month - 1];

    public IEnumerator<bool> GetEnumerator() => _months.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(bool item) => _months.Add(item);

    public void Clear() => _months.Clear();

    public bool Contains(bool item) => _months.Contains(item);

    public void CopyTo(bool[] array, int arrayIndex) => _months.CopyTo(array, arrayIndex);

    public bool Remove(bool item) => _months.Remove(item);

    public int Count => _months.Count;
    public bool IsReadOnly => _months.IsReadOnly;

    public bool this[int index]
    {
        get => _months[index];
        // ReSharper disable once UnusedMember.Global
        set => _months[index] = value;
    }

    internal bool[] GetMonths() => _months.ToArray();
}