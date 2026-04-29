using Microsoft.AspNetCore.Mvc;
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

        // POST: api/reservas
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

        // GET: api/reservas
        [HttpGet]
        public IActionResult ObtenerReservas()
        {
            var reservas = _context.Reservas.ToList();
            return Ok(reservas);
        }
    }
}