using sprint18_Auth.Models.DTOs;

namespace sprint18_Auth.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterModel model);
    Task<AuthResponse> LoginAsync(LoginModel model);
    Task<AuthResponse> RefreshAsync(RefreshRequest request);
    Task<bool> LogoutAsync(string refreshToken);
}