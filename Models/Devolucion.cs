using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Devolucion
    {
        [Key]
        public int IdDevolucion { get; set; }
        public int IdPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
        [MaxLength(500)]
        public string? Observaciones { get; set; }
        public Prestamo? Prestamo { get; set; }
    }
}