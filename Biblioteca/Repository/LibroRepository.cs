using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repository
{
    public class LibroRepository : IRepository<Libro>
    {
        private BibliotecaContext _context;

        public LibroRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Libro>> Get() =>
            await _context.Libros.ToListAsync();

        public async Task<Libro> GetById(int id) =>
            await _context.Libros.FindAsync(id);

        public async Task Add(Libro libro) =>
            await _context.Libros.AddAsync(libro);

        public void Update(Libro libro)
        {
            _context.Libros.Attach(libro);
            _context.Entry(libro).State = EntityState.Modified;
        }

        public void Delete(Libro libro) =>
           _context.Libros.Remove(libro);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<Libro> Search(Func<Libro, bool> filter) =>
        _context.Libros.Where(filter).ToList();
    }
}

