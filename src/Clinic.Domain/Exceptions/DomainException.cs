namespace Clinic.Domain.Exceptions;

/// <summary>
/// Exception thrown when a domain rule or invariant is violated.
/// Use this to signal business logic errors that are recoverable and
/// meaningful to the application layer.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Initialises a new instance of <see cref="DomainException"/> with the specified message.
    /// </summary>
    /// <param name="message">A human-readable description of the violated rule.</param>
    public DomainException(string message) : base(message) { }

    /// <summary>
    /// Initialises a new instance of <see cref="DomainException"/> with the specified message
    /// and a reference to the inner exception that caused this exception.
    /// </summary>
    /// <param name="message">A human-readable description of the violated rule.</param>
    /// <param name="inner">The exception that is the cause of the current exception.</param>
    public DomainException(string message, Exception inner) : base(message, inner) { }
}
