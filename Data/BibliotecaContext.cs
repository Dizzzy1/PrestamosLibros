
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

namespace Biblioteca.Data
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options)
            : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Devolucion> Devoluciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Estudiante)
                .WithMany(e => e.Prestamos)
                .HasForeignKey(p => p.IdEstudiante);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro)
                .WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.IdLibro);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Devolucion)
                .WithOne(d => d.Prestamo)
                .HasForeignKey<Devolucion>(d => d.IdPrestamo);
        }
    }
}
