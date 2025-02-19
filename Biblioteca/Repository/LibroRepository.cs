using Biblioteca.DTOs;
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

        public async Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios()
        {
            return await _context.Libros
                .Select(l => new LibroVentaDTO
                {
                    Titulo = l.Titulo,
                    Precio = l.Precio
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado()
        {
            return await _context.Libros
                .GroupBy(l => l.Descatalogado)
                .Select(g => new LibroGroupDTO
                {
                    Descatalogado = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<Libro>> GetLibrosPaginados(int desde, int hasta)
        {
            if (hasta < desde)
            {
                throw new ArgumentException("El máximo no puede ser inferior al mínimo");
            }

            return await _context.Libros
                .Skip(desde - 1)
                .Take(hasta - desde + 1)
                .ToListAsync();
        }
        public async Task<IEnumerable<Libro>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax)
        {
            return await _context.Libros
                .Where(libro => libro.Precio >= precioMin && libro.Precio <= precioMax)
                .ToListAsync();
        }
        public async Task<IEnumerable<Libro>> GetLibrosOrdenadosPorTitulo(bool ascendente)
        {
            if (ascendente)
            {
                return await _context.Libros.OrderBy(x => x.Titulo).ToListAsync();
            }
            else
            {
                return await _context.Libros.OrderByDescending(x => x.Titulo).ToListAsync();
            }
        }
        public async Task<IEnumerable<Libro>> GetLibrosPorTituloContiene(string texto)
        {
            return await _context.Libros
                                 .Where(x => x.Titulo.Contains(texto))
                                 .ToListAsync();
        }
        public async Task Add(Libro libro) =>
            await _context.Libros.AddAsync(libro);

        public void Delete(Libro libro) =>
           _context.Libros.Remove(libro);

        public async Task<Libro> GetLibroPorId(int id)
        {
            return await _context.Libros.FindAsync(id);
        }
        public async Task DeleteLibro(Libro libro)
        {
            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
        }

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public async Task<bool> LibroExists(int id) =>
        await _context.Libros.AnyAsync(l => l.IdLibro == id);

        public async Task<bool> ExisteAutor(int autorId) =>
            await _context.Autores.AnyAsync(a => a.IdAutor == autorId);

        public async Task<bool> ExisteEditorial(int editorialId) =>
            await _context.Editoriales.AnyAsync(e => e.IdEditorial == editorialId);

        public IEnumerable<Libro> Search(Func<Libro, bool> filter) =>
            _context.Libros.Where(filter).ToList();

        public Task<IEnumerable<object>> GetAutoresConDetalles()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Libro>> IRepository<Libro>.GetAutoresConDetalles()
        {
            throw new NotImplementedException();
        }

        public Task<AutorLibroDTO?> GetAutorLibrosSelect(int id)
        {
            throw new NotImplementedException();
        }

        public Task<EditorialLibroDTO?> GetEditorialesLibrosEager(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Libro libro)
        {
            _context.Libros.Attach(libro);
            _context.Entry(libro).State = EntityState.Modified;
        }

    }
}

