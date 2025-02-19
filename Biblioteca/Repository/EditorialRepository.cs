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

        public async Task<IEnumerable<Editorial>> Get() =>
            await _context.Editoriales.ToListAsync();

        public async Task<Editorial> GetById(int id) =>
            await _context.Editoriales.FindAsync(id);

        public async Task<EditorialLibroDTO?> GetEditorialesLibrosEager(int id)
        {
            var editorial = await _context.Editoriales
                .Include(e => e.Libros)
                .Where(e => e.IdEditorial == id)
                .Select(e => new EditorialLibroDTO
                {
                    IdEditorial = e.IdEditorial,
                    Nombre = e.Nombre,
                    Libros = e.Libros.Select(l => new LibroItemDTO
                    {
                        Titulo = l.Titulo
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return editorial;
        }

        public async Task<IEnumerable<Editorial>> GetEditorialesOrdenadasPorNombre(bool ascendente)
        {
            if (ascendente)
            {
                return await _context.Editoriales.OrderBy(x => x.Nombre).ToListAsync();
            }
            else
            {
                return await _context.Editoriales.OrderByDescending(x => x.Nombre).ToListAsync();
            }
        }

        public async Task<IEnumerable<Editorial>> GetEditorialesPorNombreContiene(string texto)
        {
            return await _context.Editoriales
                                 .Where(x => x.Nombre.Contains(texto))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Editorial>> GetEditorialesPaginadas(int desde, int hasta)
        {
            if (hasta < desde)
            {
                throw new ArgumentException("El máximo no puede ser inferior al mínimo");
            }

            return await _context.Editoriales
                .Skip(desde - 1)
                .Take(hasta - desde + 1)
                .ToListAsync();
        }
        public async Task Add(Editorial editorial) =>
            await _context.Editoriales.AddAsync(editorial);

        public void Update(Editorial editorial)
        {
            _context.Editoriales.Attach(editorial);
            _context.Entry(editorial).State = EntityState.Modified;
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

        Task<bool> IEditorialRepository.ExisteEditorial(int editorialId)
        {
            throw new NotImplementedException();
        }
        
    }
}
