using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sprint18_Auth.Auth;
using sprint18_Auth.Models.DTOs;
using sprint18_Auth.Services.Interfaces;

namespace sprint18_Auth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _authService.RegisterAsync(model);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.RefreshTokenExpiresAt
            };
            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);

           
            return Ok(new
            {
                accessToken = result.AccessToken,
                email = result.Email,
                userId = result.UserId,
                role = result.Role,
                accessTokenExpiresAt = result.AccessTokenExpiresAt
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await _authService.LoginAsync(model);

            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.RefreshTokenExpiresAt
            };
            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);
            
            return Ok(new
            {
                accessToken = result.AccessToken,
                email = result.Email,
                userId = result.UserId,
                role = result.Role,
                accessTokenExpiresAt = result.AccessTokenExpiresAt
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token is missing" });

        var request = new RefreshRequest { RefreshToken = refreshToken };

        try
        {
            var result = await _authService.RefreshAsync(request);
            
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.RefreshTokenExpiresAt
            };
            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);

            return Ok(new
            {
                accessToken = result.AccessToken,
                email = result.Email,
                userId = result.UserId,
                role = result.Role,
                accessTokenExpiresAt = result.AccessTokenExpiresAt
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await _authService.LogoutAsync(refreshToken);
        }

        Response.Cookies.Delete("refreshToken");

        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize(Policy = Policies.RequireAdminRole)]
    [HttpGet("admin")]
    public IActionResult AdminOnly() => Ok("Only for admins");
}