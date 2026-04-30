
// DTOs/ActualizarEstadoDto.cs
using System.ComponentModel.DataAnnotations;

public class ActualizarEstadoDto
{
    [Required, RegularExpression("pendiente|confirmada|cancelada")]
    public string Estado { get; set; } = string.Empty;
}