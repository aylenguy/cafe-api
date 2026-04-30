// DTOs/LoginRequestDto.cs
using System.ComponentModel.DataAnnotations;

public class LoginRequestDto
{
    [Required] public string Usuario { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

