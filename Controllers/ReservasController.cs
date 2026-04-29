using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CafeApi.Data;
using CafeApi.Models;

namespace CafeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/reservas — público
        [HttpPost]
        public async Task<IActionResult> CrearReserva(Reserva reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (reserva.Fecha < DateTime.Now)
                return BadRequest(new { mensaje = "La fecha debe ser futura" });

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return Ok(reserva);
        }

        // GET: api/reservas — solo admin
        [Authorize]
        [HttpGet]
        public IActionResult ObtenerReservas()
        {
            var reservas = _context.Reservas.ToList();
            return Ok(reservas);
        }

        // DELETE: api/reservas/{id} — solo admin
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound(new { mensaje = "Reserva no encontrada" });

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = "Reserva eliminada" });
        }
    }
}