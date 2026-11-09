using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Estudiante
    {
        [Key]
        public int IdEstudiante { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string Matricula { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Celular { get; set; }

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}