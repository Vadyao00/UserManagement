using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Name { get; set; }
    public DateTimeOffset? LastLogin { get; set; }
    public DateTimeOffset RegistrationTime { get; set; } = DateTimeOffset.Now;
    public string Status { get; set; } = "active";
    public DateTimeOffset? DeletedAt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
}