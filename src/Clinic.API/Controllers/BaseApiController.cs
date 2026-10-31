namespace Clinic.API.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base API controller providing lazy access to the MediatR <see cref="ISender"/> pipeline.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    private ISender? _mediator;

    /// <summary>Gets the MediatR sender resolved from the current HTTP request scope.</summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
