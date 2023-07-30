using Microsoft.AspNetCore.Mvc;
namespace AgroExpressAPI.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseApiController
{
}
