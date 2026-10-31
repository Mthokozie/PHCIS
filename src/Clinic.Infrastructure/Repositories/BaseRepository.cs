namespace Clinic.Infrastructure.Repositories;

using Clinic.Domain.Common;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Generic base repository providing common CRUD operations for any <see cref="BaseEntity"/> aggregate.
/// Concrete repositories inherit this class and extend it with domain-specific queries.
/// </summary>
/// <typeparam name="T">Domain entity type that inherits <see cref="BaseEntity"/>.</typeparam>
public abstract class BaseRepository<T> where T : BaseEntity
{
    /// <summary>The underlying EF Core database context.</summary>
    protected readonly ApplicationDbContext Context;

    /// <summary>The <see cref="DbSet{TEntity}"/> for the entity type <typeparamref name="T"/>.</summary>
    protected readonly DbSet<T> DbSet;

    /// <summary>Initialises a new instance of <see cref="BaseRepository{T}"/>.</summary>
    /// <param name="context">The application database context.</param>
    protected BaseRepository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    /// <summary>Retrieves an entity by its primary key, or <c>null</c> if not found.</summary>
    /// <param name="id">The entity identifier.</param>
    /// <param name="ct">Optional cancellation token.</param>
    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbSet.FindAsync(new object[] { id }, ct);

    /// <summary>Adds a new entity to the change tracker (persisted on SaveChanges).</summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="ct">Optional cancellation token.</param>
    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    /// <summary>Marks an existing entity as modified in the change tracker.</summary>
    /// <param name="entity">The entity to update.</param>
    public virtual void Update(T entity)
        => DbSet.Update(entity);

    /// <summary>Marks an entity for deletion in the change tracker.</summary>
    /// <param name="entity">The entity to remove.</param>
    public virtual void Remove(T entity)
        => DbSet.Remove(entity);
}
