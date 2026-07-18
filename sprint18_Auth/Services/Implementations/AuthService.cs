using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using sprint18_Auth.Data;
using sprint18_Auth.Helpers;
using sprint18_Auth.Models.DTOs;
using sprint18_Auth.Models.Entities;
using sprint18_Auth.Services.Interfaces;

namespace sprint18_Auth.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        JwtHelper jwtHelper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterModel model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
            throw new InvalidOperationException("User with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }

        await EnsureRoleExistsAsync("User");
        await _userManager.AddToRoleAsync(user, "User");

        var roles = await _userManager.GetRolesAsync(user);
        return await GenerateAuthResponseAsync(user, roles);
    }

    public async Task<AuthResponse> LoginAsync(LoginModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
            throw new UnauthorizedAccessException("Invalid email or password.");

        var roles = await _userManager.GetRolesAsync(user);
        return await GenerateAuthResponseAsync(user, roles);
    }

    public async Task<AuthResponse> RefreshAsync(RefreshRequest request)
    {
        var storedToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

        if (storedToken == null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        if (!storedToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token is expired or revoked.");

        var user = storedToken.User;
        if (user == null)
            throw new UnauthorizedAccessException("User not found.");

        storedToken.IsRevoked = true;
        storedToken.RevokedAt = DateTime.UtcNow;

        var roles = await _userManager.GetRolesAsync(user);
        return await GenerateAuthResponseAsync(user, roles);
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken == null)
            return false;

        storedToken.IsRevoked = true;
        storedToken.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task EnsureRoleExistsAsync(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    private async Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser user, IList<string> roles)
    {
        var accessToken = _jwtHelper.GenerateJwtToken(user, roles);
        var refreshToken = RefreshTokenHelper.GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtHelper.GetRefreshTokenExpirationDays()),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken, 
            Email = user.Email ?? string.Empty,
            UserId = user.Id,
            Role = roles.FirstOrDefault() ?? "User",
            AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_jwtHelper.GetJwtExpirationMinutes()),
            RefreshTokenExpiresAt = refreshTokenEntity.ExpiresAt
        };
    }
}