using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser
{
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime RegistrationTime { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "active";
    public DateTime? DeletedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
}