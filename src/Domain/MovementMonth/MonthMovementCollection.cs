using System.Collections;

namespace Domain.MovementMonth;

public class MonthMovementCollection : ICollection<MonthMovement>
{
    private readonly ICollection<MonthMovement> _monthMovements;

    private MonthMovementCollection(ICollection<MonthMovement>? monthMovements = null) => _monthMovements = monthMovements ?? new List<MonthMovement>();

    public static MonthMovementCollection Init(ICollection<MonthMovement>? monthMovements = null) => new(monthMovements);

    public IEnumerator<MonthMovement> GetEnumerator() => _monthMovements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(MonthMovement item) => _monthMovements.Add(item);

    public void Clear() => _monthMovements.Clear();

    public bool Contains(MonthMovement item) => _monthMovements.Contains(item);

    public void CopyTo(MonthMovement[] array, int arrayIndex) => _monthMovements.CopyTo(array, arrayIndex);

    public bool Remove(MonthMovement item) => _monthMovements.Remove(item);

    public int Count => _monthMovements.Count;
    public bool IsReadOnly => _monthMovements.IsReadOnly;

    internal MonthMovement? Get(int id) => _monthMovements.SingleOrDefault(mm => mm.Id == id);
}
