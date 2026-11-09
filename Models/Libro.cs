using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Models
{
    public class Libro
    {
        [Key]
        public int IdLibro { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Autor { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ISBN { get; set; } = string.Empty;
        public int CantidadTotal { get; set; }
        public int CantidadDisponible { get; set; }
        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}