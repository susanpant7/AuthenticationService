using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationSystem.Controllers;

[ApiController]
[Route("api/test-auth")]
public class TestAuthController : ControllerBase
{
    [HttpGet("test")]
    [AllowAnonymous]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        try
        {
            throw new BadHttpRequestException("You are not authorized!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return Ok("You are authenticated!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("You are and admin!");
    }
}