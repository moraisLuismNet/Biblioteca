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

    }
}
