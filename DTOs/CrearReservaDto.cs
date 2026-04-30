// DTOs/CrearReservaDto.cs
using System.ComponentModel.DataAnnotations;

public class CrearReservaDto
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Phone]
    public string? Telefono { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Range(1, 20)]
    public int Personas { get; set; } = 1;

    [MaxLength(500)]
    public string? Notas { get; set; }
    
}
