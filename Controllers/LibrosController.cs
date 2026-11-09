
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
        {
            return await _context.Libros.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
                return NotFound("Libro no encontrado.");

            return libro;
        }

        [HttpPost]
        public async Task<ActionResult<Libro>> CrearLibro(Libro libro)
        {
            if (libro.CantidadTotal < 0)
                return BadRequest("La cantidad no puede ser negativa.");

            libro.CantidadDisponible = libro.CantidadTotal;

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLibro),
                new { id = libro.IdLibro }, libro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarLibro(int id, Libro libro)
        {
            if (id != libro.IdLibro)
                return BadRequest("El ID no coincide.");

            var existente = await _context.Libros.FindAsync(id);

            if (existente == null)
                return NotFound("Libro no encontrado.");

            int prestados = existente.CantidadTotal - existente.CantidadDisponible;

            if (libro.CantidadTotal < prestados)
                return BadRequest("La cantidad total no puede ser menor que los libros prestados.");

            existente.Titulo = libro.Titulo;
            existente.Autor = libro.Autor;
            existente.ISBN = libro.ISBN;
            existente.CantidadTotal = libro.CantidadTotal;
            existente.CantidadDisponible = libro.CantidadTotal - prestados;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);

            if (libro == null)
                return NotFound("Libro no encontrado.");

            if (libro.CantidadDisponible != libro.CantidadTotal)
                return BadRequest("No se puede eliminar un libro que tiene préstamos activos.");

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
