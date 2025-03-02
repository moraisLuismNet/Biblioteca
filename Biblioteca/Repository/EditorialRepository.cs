using Biblioteca.DTOs;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repository
{
    public class EditorialRepository : IBaseRepository<Editorial>, IEditorialRepository
    {
        private BibliotecaContext _context;

        public EditorialRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EditorialDTO>> Get()
        {
            var editoriales = await (from x in _context.Editoriales
                                     select new EditorialDTO
                                     {
                                         IdEditorial = x.IdEditorial,
                                         NombreEditorial = x.Nombre,
                                         TotalLibros = x.Libros.Count()
                                     }).ToListAsync();
            return editoriales;
        }

        public async Task<Editorial> GetById(int id)
        {
            return await _context.Editoriales
                .Include(a => a.Libros)
                .FirstOrDefaultAsync(a => a.IdEditorial == id);
        }

        public async Task<IEnumerable<EditorialInsertDTO>> GetEditorialesOrdenadasPorNombre(bool ascendente)
        {
            if (ascendente)
            {
                return await _context.Editoriales
                    .OrderBy(x => x.Nombre)
                    .Select(a => new EditorialInsertDTO { NombreEditorial = a.Nombre })
                    .ToListAsync();
            }

            else
            {
                return await _context.Editoriales
                    .OrderByDescending(x => x.Nombre)
                    .Select(a => new EditorialInsertDTO { NombreEditorial = a.Nombre })
                    .ToListAsync();

            }
        }

        public async Task<EditorialLibroDTO?> GetEditorialLibrosSelect(int id)
        {
            return await _context.Editoriales
                .Where(x => x.IdEditorial == id)
                .Select(x => new EditorialLibroDTO
                {
                    IdEditorial = x.IdEditorial,
                    NombreEditorial = x.Nombre,
                    TotalLibros = x.Libros.Count(),
                    Libros = x.Libros.Select(y => new LibroItemDTO
                    {
                        IdLibro = y.IdLibro,
                        Titulo = y.Titulo,
                        Precio = y.Precio
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EditorialInsertDTO>> GetEditorialesPorNombreContiene(string texto)
        {
            return await _context.Editoriales
                                 .Where(x => x.Nombre.Contains(texto))
                                 .Select(a => new EditorialInsertDTO { NombreEditorial = a.Nombre })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<EditorialInsertDTO>> GetEditorialesPaginadas(int desde, int hasta)
        {
            if (hasta < desde)
            {
                throw new ArgumentException("El máximo no puede ser inferior al mínimo");
            }

            return await _context.Editoriales
                .Skip(desde - 1)
                .Take(hasta - desde + 1)
                .Select(a => new EditorialInsertDTO { NombreEditorial = a.Nombre })
                .ToListAsync();
        }
        
        public async Task Add(EditorialInsertDTO editorialInsertDTO)
        {
            var editorial = new Editorial
            {
                Nombre = editorialInsertDTO.NombreEditorial 
            };

            _context.Editoriales.Add(editorial);
            await _context.SaveChangesAsync();
        }

        public async Task Update(EditorialUpdateDTO editorialUpdateDTO)
        {
            var editorial = await _context.Editoriales
                .AsTracking()
                .FirstOrDefaultAsync(e => e.IdEditorial == editorialUpdateDTO.IdEditorial);

            if (editorial != null)
            {
                editorial.Nombre = editorialUpdateDTO.NombreEditorial;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Editorial no encontrada");
            }
        }

        public void Delete(Editorial editorial) =>
           _context.Editoriales.Remove(editorial);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<Editorial> Search(Func<Editorial, bool> filter) =>
        _context.Editoriales.Where(filter).ToList();

        public async Task<IEnumerable<Editorial>> GetAutoresConDetalles()
        {
            return await _context.Editoriales.Include(e => e.Libros).ToListAsync();
        }

    }
}
