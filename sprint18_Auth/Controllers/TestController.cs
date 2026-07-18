using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace sprint18_Auth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("public")]
    public IActionResult Public() => Ok("Public endpoint - доступно всім");

    [Authorize]
    [HttpGet("protected")]
    public IActionResult Protected() => Ok($"Protected endpoint - доступно для {User.Identity?.Name}");
}