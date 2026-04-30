// Models/Reserva.cs
namespace CafeApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }       // ← agregar
        public DateTime Fecha { get; set; }
        public int Personas { get; set; } = 1;   // ← agregar
        public string? Notas { get; set; }        // ← agregar
        public string Estado { get; set; } = "pendiente"; // ← agregar
    }
}