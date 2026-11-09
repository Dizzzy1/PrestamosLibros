using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Prestamo
    {
        [Key]
        public int IdPrestamo { get; set; }
        public int IdEstudiante { get; set; }
        public int IdLibro { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaVencimiento { get; set; }

        [MaxLength(20)]
        public string Estado { get; set; } = "Activo";
        public Estudiante? Estudiante { get; set; }
        public Libro? Libro { get; set; }
        public Devolucion? Devolucion { get; set; }
    }
}