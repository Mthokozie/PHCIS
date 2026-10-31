namespace Clinic.Domain.Exceptions;

/// <summary>
/// Exception thrown when a requested entity cannot be found in the data store.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initialises a new instance of <see cref="NotFoundException"/>.
    /// </summary>
    /// <param name="name">The name of the entity type that was not found (e.g. "Patient").</param>
    /// <param name="key">The key (e.g. Id) that was used in the lookup.</param>
    public NotFoundException(string name, object key)
        : base($"Entity '{name}' with key '{key}' was not found.") { }
}
