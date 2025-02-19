using Biblioteca.DTOs;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repository
{
    public class AutorRepository : IBaseRepository<Autor>, IAutorRepository
    {
        private BibliotecaContext _context;

        public AutorRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autor>> Get() =>
            await _context.Autores.ToListAsync();

        public async Task<Autor> GetById(int id) =>
            await _context.Autores.FindAsync(id);

        public async Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles()
        {
            return await _context.Autores
                .Include(a => a.Libros) 
                .Select(a => new AutorLibroDTO
                {
                    IdAutor = a.IdAutor,
                    Nombre = a.Nombre,
                    TotalLibros = a.Libros.Count,
                    PromedioPrecios = a.Libros.Any() ? a.Libros.Average(l => l.Precio) : 0,
                    Libros = a.Libros.Select(l => new LibroItemDTO
                    {
                        Titulo = l.Titulo
                    }).ToList()
                }).ToListAsync(); 
        }

        public async Task<AutorLibroDTO?> GetAutorLibrosSelect(int id)
        {
            return await _context.Autores
                .Where(x => x.IdAutor == id)
                .Select(x => new AutorLibroDTO
                {
                    IdAutor = x.IdAutor,
                    Nombre = x.Nombre,
                    TotalLibros = x.Libros.Count(),
                    PromedioPrecios = x.Libros.Any() ? x.Libros.Average(libro => (decimal?)libro.Precio) ?? 0 : 0,
                    Libros = x.Libros.Select(y => new LibroItemDTO
                    {
                        Titulo = y.Titulo
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Autor>> GetAutoresOrdenadosPorNombre(bool ascendente)
        {
            if (ascendente)
            {
                return await _context.Autores.OrderBy(x => x.Nombre).ToListAsync();
            }
            else
            {
                return await _context.Autores.OrderByDescending(x => x.Nombre).ToListAsync();
            }
        }

        public async Task<IEnumerable<Autor>> GetAutoresPorNombreContiene(string texto)
        {
            return await _context.Autores
                                 .Where(x => x.Nombre.Contains(texto))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Autor>> GetAutoresPaginados(int desde, int hasta)
        {
            if (hasta < desde)
            {
                throw new ArgumentException("El máximo no puede ser inferior al mínimo");
            }

            return await _context.Autores
                .Skip(desde - 1)
                .Take(hasta - desde + 1)
                .ToListAsync();
        }
        public async Task Add(Autor autor) =>
            await _context.Autores.AddAsync(autor);

        public void Update(Autor autor)
        {
            _context.Autores.Attach(autor);
            _context.Entry(autor).State = EntityState.Modified;
        }

        public void Delete(Autor autor) =>
           _context.Autores.Remove(autor);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<Autor> Search(Func<Autor, bool> filter) =>
        _context.Autores.AsQueryable().Where(filter).ToList();

        Task<IEnumerable<Libro>> IAutorRepository.GetAutoresConDetalles()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteAutor(int autorId)
        {
            throw new NotImplementedException();
        }

    }
}

