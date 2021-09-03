using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MovementMonth
{
    public class MonthMovementCollection : ICollection<MonthMovement>
    {
        private readonly ICollection<MonthMovement> _monthMovements;

        private MonthMovementCollection(ICollection<MonthMovement>? monthMovements = null)
        {
            _monthMovements = monthMovements ?? new List<MonthMovement>();
        }

        public static MonthMovementCollection Init(ICollection<MonthMovement>? monthMovements = null)
        {
            return new MonthMovementCollection(monthMovements);
        }

        public IEnumerator<MonthMovement> GetEnumerator()
        {
            return _monthMovements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(MonthMovement item)
        {
            _monthMovements.Add(item);
        }

        public void Clear()
        {
            _monthMovements.Clear();
        }

        public bool Contains(MonthMovement item)
        {
            return _monthMovements.Contains(item);
        }

        public void CopyTo(MonthMovement[] array, int arrayIndex)
        {
            _monthMovements.CopyTo(array, arrayIndex);
        }

        public bool Remove(MonthMovement item)
        {
            return _monthMovements.Remove(item);
        }

        public int Count => _monthMovements.Count;
        public bool IsReadOnly => _monthMovements.IsReadOnly;

        internal MonthMovement? Get(int id)
        {
            return _monthMovements.SingleOrDefault(mm => mm.Id == id);
        }
    }
}