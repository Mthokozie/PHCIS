namespace Clinic.Application.Common.Interfaces;

/// <summary>
/// Unit of Work contract for committing transactional changes.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>Persists all pending changes to the underlying store.</summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
