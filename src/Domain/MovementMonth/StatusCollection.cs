using System.Collections;

namespace Domain.MovementMonth
{
    public class StatusCollection : ICollection<Status>
    {
        private readonly ICollection<Status> _statuses;

        private StatusCollection(ICollection<Status>? statuses = null)
        {
            _statuses = statuses ?? new List<Status>();
        }

        public static StatusCollection Init(ICollection<Status>? statuses = null)
        {
            return new StatusCollection(statuses);
        }

        public IEnumerator<Status> GetEnumerator()
        {
            return _statuses.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(Status item)
        {
            _statuses.Add(item);
        }

        public void Clear()
        {
            _statuses.Clear();
        }

        public bool Contains(Status item)
        {
            return _statuses.Contains(item);
        }

        public void CopyTo(Status[] array, int arrayIndex)
        {
            _statuses.CopyTo(array, arrayIndex);
        }

        public bool Remove(Status item)
        {
            return _statuses.Remove(item);
        }

        public int Count => _statuses.Count;
        public bool IsReadOnly => _statuses.IsReadOnly;
        
        internal Status? GetByDay(int day)
        {
            return _statuses.SingleOrDefault(s => s.Day == day);
        }
    }
}