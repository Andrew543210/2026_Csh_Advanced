using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using sprint18_Auth.Auth;
using sprint18_Auth.Models.Entities;

namespace sprint18_Auth.Helpers;

public class JwtHelper
{
    private readonly JwtSettings _jwtSettings;

    public JwtHelper(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateJwtToken(ApplicationUser user, IList<string> roles)
    {
        var secretKey = _jwtSettings.Secret ?? throw new ArgumentNullException("JwtSettings:Secret is missing");
        var issuer = _jwtSettings.Issuer ?? "Sprint18_Auth";
        var audience = _jwtSettings.Audience ?? "Sprint18_Clients";
        var expirationMinutes = _jwtSettings.ExpirationInMinutes;

        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("fullName", $"{user.FirstName} {user.LastName}".Trim())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public int GetJwtExpirationMinutes()
    {
        return _jwtSettings.ExpirationInMinutes;
    }

    public int GetRefreshTokenExpirationDays()
    {
        return _jwtSettings.RefreshExpirationInDays;
    }
}