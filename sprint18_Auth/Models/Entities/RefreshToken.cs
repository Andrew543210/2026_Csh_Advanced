using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace sprint18_Auth.Models.Entities;

[Index(nameof(Token), IsUnique = true)]
public class RefreshToken
{
    public int Id { get; set; }

    [Required]
    [MaxLength(128)]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string UserId { get; set; } = string.Empty;

    public ApplicationUser? User { get; set; }

    public DateTime ExpiresAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    public bool IsActive => !IsRevoked && !IsExpired;
}