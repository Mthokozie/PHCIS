namespace Clinic.Domain.Common;

/// <summary>
/// Marker interface for domain events.
/// All domain events should implement this interface so they can be
/// dispatched through the application's event infrastructure.
/// </summary>
public interface IDomainEvent { }
