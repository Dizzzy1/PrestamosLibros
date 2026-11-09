
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudiantesController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public EstudiantesController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound("Estudiante no encontrado.");

            return estudiante;
        }

        [HttpPost]
        public async Task<ActionResult<Estudiante>> CrearEstudiante(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEstudiante),
                new { id = estudiante.IdEstudiante }, estudiante);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.IdEstudiante)
                return BadRequest("El ID no coincide.");

            _context.Entry(estudiante).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound("Estudiante no encontrado.");

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
