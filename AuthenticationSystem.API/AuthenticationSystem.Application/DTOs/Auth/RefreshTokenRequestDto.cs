using System.ComponentModel.DataAnnotations;

namespace AuthenticationSystem.Application.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
