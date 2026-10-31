namespace Clinic.Application.Common.Behaviours;

using MediatR;
using Microsoft.Extensions.Logging;

/// <summary>
/// MediatR pipeline behaviour that logs incoming requests and outgoing responses.
/// </summary>
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    /// <summary>Initialises a new instance of <see cref="LoggingBehaviour{TRequest, TResponse}"/>.</summary>
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("PHCIS Request: {Name} {@Request}", requestName, request);

        var response = await next();

        _logger.LogInformation("PHCIS Response: {Name} completed", requestName);
        return response;
    }
}
