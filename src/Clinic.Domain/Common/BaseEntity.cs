namespace Clinic.Domain.Common;

/// <summary>
/// Abstract base class for all domain entities.
/// Provides a unique identifier, audit timestamps, and domain event tracking.
/// </summary>
public abstract class BaseEntity
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>Gets the unique identifier for this entity.</summary>
    public Guid Id { get; protected set; }

    /// <summary>Gets or sets the UTC date and time when this entity was created.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Gets or sets the UTC date and time when this entity was last updated.</summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Gets a read-only collection of domain events that have been raised by this entity
    /// since the last time they were cleared.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to this entity's pending event collection.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all pending domain events from this entity.
    /// Should be called after the events have been dispatched.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}
