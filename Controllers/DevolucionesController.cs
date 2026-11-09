
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevolucionesController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public DevolucionesController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Devolucion>>> GetDevoluciones()
        {
            return await _context.Devoluciones
                .Include(d => d.Prestamo)
                    .ThenInclude(p => p!.Estudiante)
                .Include(d => d.Prestamo)
                    .ThenInclude(p => p!.Libro)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Devolucion>> CrearDevolucion(Devolucion devolucion)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Libro)
                .FirstOrDefaultAsync(p => p.IdPrestamo == devolucion.IdPrestamo);

            if (prestamo == null)
                return NotFound("El préstamo no existe.");

            if (prestamo.Estado == "Devuelto")
                return BadRequest("Este préstamo ya fue devuelto.");

            if (prestamo.Libro == null)
                return BadRequest("No se encontró el libro del préstamo.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            devolucion.FechaDevolucion = DateTime.Now;
            prestamo.Estado = "Devuelto";
            prestamo.Libro.CantidadDisponible++;

            _context.Devoluciones.Add(devolucion);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(devolucion);
        }
    }
}
