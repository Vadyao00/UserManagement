using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public record UserForAuthenticationDto
{
    [Required(ErrorMessage = "Email is required")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "Password name is required")]
    public string? Password { get; init; }
}