namespace ECommerce.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; protected set; }

    public void MarkUpdated() => UpdatedAtUtc = DateTime.UtcNow;
}
