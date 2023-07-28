using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AgroExpressAPI.Controllers;
[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}