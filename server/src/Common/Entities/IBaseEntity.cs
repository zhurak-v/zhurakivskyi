namespace Common.Entities;

public interface IBaseEntity
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; }

    void SetUpdatedAt();
}