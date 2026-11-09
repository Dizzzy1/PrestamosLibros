
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public PrestamosController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamos()
        {
            return await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Prestamo>> GetPrestamo(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                .Include(p => p.Devolucion)
                .FirstOrDefaultAsync(p => p.IdPrestamo == id);

            if (prestamo == null)
                return NotFound("Préstamo no encontrado.");

            return prestamo;
        }

        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosActivos()
        {
            return await _context.Prestamos
                .Include(p => p.Estudiante)
                .Include(p => p.Libro)
                .Where(p => p.Estado == "Activo")
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Prestamo>> CrearPrestamo(Prestamo prestamo)
        {
            var estudiante = await _context.Estudiantes.FindAsync(prestamo.IdEstudiante);
            var libro = await _context.Libros.FindAsync(prestamo.IdLibro);

            if (estudiante == null)
                return BadRequest("El estudiante no existe.");

            if (libro == null)
                return BadRequest("El libro no existe.");

            if (libro.CantidadDisponible <= 0)
                return BadRequest("No hay ejemplares disponibles.");

            if (prestamo.FechaVencimiento.Date < DateTime.Today)
                return BadRequest("La fecha de vencimiento no puede ser anterior a hoy.");

            libro.CantidadDisponible--;

            prestamo.FechaPrestamo = DateTime.Now;
            prestamo.Estado = "Activo";

            _context.Prestamos.Add(prestamo);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrestamo),
                new { id = prestamo.IdPrestamo }, prestamo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);

            if (prestamo == null)
                return NotFound("Préstamo no encontrado.");

            if (prestamo.Estado == "Activo")
                return BadRequest("Primero debes registrar la devolución del préstamo.");

            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
