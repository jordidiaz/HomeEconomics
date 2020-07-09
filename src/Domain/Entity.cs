namespace Domain
{
    public abstract class Entity
    {
        public int Id { get; private set; }

        public void SetIdentity(int id)
        {
            Id = id;
        }
    }
}
