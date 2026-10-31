namespace Clinic.Application.Common.Exceptions;

/// <summary>
/// Exception thrown when an operation conflicts with existing state (e.g., duplicate booking).
/// </summary>
public class ConflictException : Exception
{
    /// <summary>Initialises a new instance of <see cref="ConflictException"/>.</summary>
    public ConflictException(string message) : base(message) { }
}
