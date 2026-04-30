using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CafeApi.Data;
using CafeApi.Models;

namespace CafeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReservasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReservasController> _logger;

        public ReservasController(ApplicationDbContext context, ILogger<ReservasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/reservas — público
        [HttpPost]
        [ProducesResponseType(typeof(Reserva), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Fecha.ToUniversalTime() <= DateTime.UtcNow)
                return BadRequest(new { mensaje = "La fecha debe ser futura." });

            // Validación de negocio: máximo N reservas por franja horaria
            var desde = dto.Fecha.AddMinutes(-30);
            var hasta = dto.Fecha.AddMinutes(30);

            var reservasEnFranja = await _context.Reservas
                .CountAsync(r => r.Fecha >= desde && r.Fecha <= hasta);

            if (reservasEnFranja >= 10)
                return Conflict(new { mensaje = "No hay lugares disponibles en ese horario." });

            var reserva = new Reserva
            {
                Nombre = dto.Nombre.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant(),
                Telefono = dto.Telefono?.Trim(),
                Fecha = dto.Fecha,
                Personas = dto.Personas,
                Notas = dto.Notas?.Trim()
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Nueva reserva #{Id} para {Nombre} el {Fecha}",
                reserva.Id, reserva.Nombre, reserva.Fecha);

            return CreatedAtAction(nameof(ObtenerReservaPorId),
                new { id = reserva.Id }, reserva);
        }

        // GET: api/reservas — solo admin
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Reserva>), 200)]
        public async Task<IActionResult> ObtenerReservas(
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamaño = 20)
        {
            tamaño = Math.Clamp(tamaño, 1, 100);

            var query = _context.Reservas.AsQueryable();

            if (desde.HasValue)
                query = query.Where(r => r.Fecha >= desde.Value);
            if (hasta.HasValue)
                query = query.Where(r => r.Fecha <= hasta.Value);

            var total = await query.CountAsync();
            var reservas = await query
                .OrderBy(r => r.Fecha)
                .Skip((pagina - 1) * tamaño)
                .Take(tamaño)
                .ToListAsync();

            Response.Headers["X-Total-Count"] = total.ToString();
            return Ok(reservas);
        }

        // GET: api/reservas/{id} — solo admin
        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Reserva), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ObtenerReservaPorId(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            return reserva is null
                ? NotFound(new { mensaje = $"Reserva #{id} no encontrada." })
                : Ok(reserva);
        }

        // PATCH: api/reservas/{id} — solo admin (confirmar / cancelar)
        [Authorize]
        [HttpPatch("{id:int}/estado")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ActualizarEstado(int id, [FromBody] ActualizarEstadoDto dto)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva is null)
                return NotFound(new { mensaje = $"Reserva #{id} no encontrada." });

            reserva.Estado = dto.Estado;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = $"Reserva #{id} actualizada a '{dto.Estado}'." });
        }

        // DELETE: api/reservas/{id} — solo admin
        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> EliminarReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva is null)
                return NotFound(new { mensaje = $"Reserva #{id} no encontrada." });

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Reserva #{Id} eliminada.", id);
            return NoContent(); // 204 es el estándar para DELETE exitoso
        }
    }
}