using Biblioteca.DTOs;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repository
{
    public class EditorialRepository : IRepository<Editorial>
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

        public Task<AutorLibroDTO?> GetAutorLibrosSelect(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Libro>> GetLibrosPaginados(int desde, int hasta)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Libro>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Libro>> GetLibrosOrdenadosPorTitulo(bool ascendente)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Libro>> GetLibrosPorTituloContiene(string texto)
        {
            throw new NotImplementedException();
        }

        public Task<Libro> GetLibroPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLibro(Libro libro)
        {
            throw new NotImplementedException();
        }

        Task<bool> IRepository<Editorial>.LibroExists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteAutor(int autorId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExisteEditorial(int editorialId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Libro>> IRepository<Editorial>.GetAutoresConDetalles()
        {
            throw new NotImplementedException();
        }
    }
}
