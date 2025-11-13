namespace Domain;

public abstract class Entity
{
    public int Id { get; private set; }

    /* For testing purposes only */
    public void SetIdentity(int id) => Id = id;
}