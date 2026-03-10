namespace MassLab.Shared.Entities;

public abstract class Entity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    protected Entity()
    { }

    protected void SetUpdatedAt() => UpdatedAt = DateTime.UtcNow;

    public override bool Equals(object? other)
    {
        if (other is not Entity entity)
            return false;

        return Id == entity.Id;
    }
    public override int GetHashCode() => Id.GetHashCode();
}
