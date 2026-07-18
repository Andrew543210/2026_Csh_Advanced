using System.ComponentModel.DataAnnotations;

namespace sprint18_Auth.Models.DTOs;

public class RefreshRequest
{
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}