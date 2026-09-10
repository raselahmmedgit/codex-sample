using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/system")]
public sealed class SystemController : ControllerBase
{
    [HttpGet("version")]
    public IActionResult Version() => Ok(new { service = "ECommerce.API", version = "1.0.0" });
}
