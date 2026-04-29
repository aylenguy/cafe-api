using System.ComponentModel.DataAnnotations;

namespace CafeApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Range(1, 20, ErrorMessage = "La cantidad debe ser entre 1 y 20")]
        public int CantidadPersonas { get; set; }
    }
}