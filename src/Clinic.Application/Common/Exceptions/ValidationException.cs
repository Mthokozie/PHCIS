namespace Clinic.Application.Common.Exceptions;

using FluentValidation.Results;

/// <summary>
/// Exception thrown when one or more FluentValidation rules fail in the application pipeline.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>Gets the dictionary of property validation errors.</summary>
    public IDictionary<string, string[]> Errors { get; }

    /// <summary>Initialises a new instance of <see cref="ValidationException"/>.</summary>
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>Initialises a new instance of <see cref="ValidationException"/> from validation failures.</summary>
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
