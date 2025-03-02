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

        public async Task<IEnumerable<AutorDTO>> Get()
        {
            var autores = await (from x in _context.Autores
                                     select new AutorDTO
                                     {
                                         IdAutor = x.IdAutor,
                                         NombreAutor = x.Nombre,
                                         TotalLibros = x.Libros.Count()
                                     }).ToListAsync();
            return autores;
        }

        public async Task<Autor> GetById(int id)
        {
            return await _context.Autores
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(a => a.IdAutor == id);
        }

        public async Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles()
        {
            return await _context.Autores
                .Include(a => a.Libros) 
                .Select(a => new AutorLibroDTO
                {
                    IdAutor = a.IdAutor,
                    NombreAutor = a.Nombre,
                    TotalLibros = a.Libros.Count,
                    PromedioPrecios = a.Libros.Any() ? a.Libros.Average(l => l.Precio) : 0,
                    Libros = a.Libros.Select(l => new LibroItemDTO
                    {
                        IdLibro = l.IdLibro,
                        Titulo = l.Titulo,
                        Precio = l.Precio
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
                    NombreAutor = x.Nombre,
                    TotalLibros = x.Libros.Count(),
                    PromedioPrecios = x.Libros.Any() ? x.Libros.Average(libro => (decimal?)libro.Precio) ?? 0 : 0,
                    Libros = x.Libros.Select(y => new LibroItemDTO
                    {
                        IdLibro = y.IdLibro,
                        Titulo = y.Titulo,
                        Precio = y.Precio
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AutorInsertDTO>> GetAutoresOrdenadosPorNombre(bool ascendente)
        {
            if (ascendente)
            {
                return await _context.Autores
                    .OrderBy(x => x.Nombre) 
                    .Select(a => new AutorInsertDTO { NombreAutor = a.Nombre }) 
                    .ToListAsync();
             }
        
            else
            {
                return await _context.Autores
                    .OrderByDescending(x => x.Nombre) 
                    .Select(a => new AutorInsertDTO { NombreAutor = a.Nombre}) 
                    .ToListAsync();

            }
        }

        public async Task<IEnumerable<AutorInsertDTO>> GetAutoresPorNombreContiene(string texto)
        {
            return await _context.Autores
                                 .Where(x => x.Nombre.Contains(texto))
                                 .Select(a => new AutorInsertDTO { NombreAutor = a.Nombre })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<AutorInsertDTO>> GetAutoresPaginados(int desde, int hasta)
        {
            if (hasta < desde)
            {
                throw new ArgumentException("El máximo no puede ser inferior al mínimo");
            }

            return await _context.Autores
                .Skip(desde - 1)
                .Take(hasta - desde + 1)
                .Select(a => new AutorInsertDTO { NombreAutor = a.Nombre })
                .ToListAsync();
        }

        public async Task Add(AutorInsertDTO autorInsertDTO)
        {
            var autor = new Autor
            {
                Nombre = autorInsertDTO.NombreAutor
            };

            _context.Autores.Add(autor);
            await _context.SaveChangesAsync();
        }

        public async Task Update(AutorUpdateDTO autorUpdateDTO)
        {
            var autor = await _context.Autores
                .AsTracking()
                .FirstOrDefaultAsync(e => e.IdAutor == autorUpdateDTO.IdAutor);

            if (autor != null)
            {
                autor.Nombre = autorUpdateDTO.NombreAutor;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Autor no encontrado");
            }
        }


        public void Delete(Autor autor) =>
           _context.Autores.Remove(autor);


        public async Task Save() =>
            await _context.SaveChangesAsync();


        public IEnumerable<Autor> Search(Func<Autor, bool> filter) =>
            _context.Autores.AsQueryable().Where(filter).ToList();

    }
}

