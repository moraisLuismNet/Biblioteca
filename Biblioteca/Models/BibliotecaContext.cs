using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Models
{
    public class BibliotecaContext : DbContext
    {
        public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
        {
        }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Editorial> Editoriales { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Operacion> Operaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relación entre Libro y Autor
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Autor)
                .WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId)
                .OnDelete(DeleteBehavior.Restrict);  // Para evitar eliminación en cascada si no la necesitas

            // Relación entre Libro y Editorial
            modelBuilder.Entity<Libro>()
                .HasOne(l => l.Editorial)
                .WithMany(e => e.Libros)
                .HasForeignKey(l => l.EditorialId)
                .OnDelete(DeleteBehavior.Restrict);  // Para evitar eliminación en cascada si no la necesitas
        }


    }
}
