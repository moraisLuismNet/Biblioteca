using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Biblioteca.Repository
{
    public class LibroRepository : IBaseRepository<Libro>, ILibroRepository
    {
        private BibliotecaContext _context;
        private readonly IGestorArchivos _gestorArchivos;

        public LibroRepository(BibliotecaContext context, IGestorArchivos gestorArchivos)
        {
            _context = context;
            _gestorArchivos = gestorArchivos;
        }

        public async Task<IEnumerable<LibroDTO>> Get()
        {
            var libros = await (from x in _context.Libros
                                select new LibroDTO
                                {
                                    Isbn = x.IdLibro,
                                    Titulo = x.Titulo,
                                    Paginas = x.Paginas,
                                    Precio = x.Precio,
                                    FotoPortada = x.FotoPortada,
                                    Descatalogado = x.Descatalogado,
                                    NombreAutor = x.Autor.Nombre,
                                    NombreEditorial = x.Editorial.Nombre
                                }).ToListAsync();
            return libros;
        }

        public async Task<LibroDTO?> GetById(int id)
        {
            var libro = await _context.Libros
                .Include(l => l.Autor)
                .Include(l => l.Editorial)
                .FirstOrDefaultAsync(l => l.IdLibro == id);

            if (libro == null)
            {
                return null;
            }

            return new LibroDTO
            {
                Isbn = libro.IdLibro,
                Titulo = libro.Titulo,
                Paginas = libro.Paginas,
                Precio = libro.Precio,
                FotoPortada = libro.FotoPortada,
                Descatalogado = libro.Descatalogado,
                NombreAutor = libro.Autor.Nombre,
                NombreEditorial = libro.Editorial.Nombre
            };
        }

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
        public async Task<IEnumerable<LibroDTO>> GetLibrosPaginados(int desde, int hasta)
        {
            if (hasta < desde)
            {
                throw new ArgumentException("El máximo no puede ser inferior al mínimo");
            }

            var libros = await (from x in _context.Libros
                                .Skip(desde - 1)
                                .Take(hasta - desde + 1)
                                select new LibroDTO
                                {
                                    Isbn = x.IdLibro,
                                    Titulo = x.Titulo,
                                    Paginas = x.Paginas,
                                    Precio = x.Precio,
                                    FotoPortada = x.FotoPortada,
                                    Descatalogado = x.Descatalogado,
                                    NombreAutor = x.Autor.Nombre,
                                    NombreEditorial = x.Editorial.Nombre
                                })
                                .ToListAsync();

            return libros;
        }
        public async Task<IEnumerable<LibroDTO>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax)
        {
            var libros = await (from x in _context.Libros
                                .Where(libro => libro.Precio >= precioMin && libro.Precio <= precioMax)
                                select new LibroDTO
                                {
                                    Isbn = x.IdLibro,
                                    Titulo = x.Titulo,
                                    Paginas = x.Paginas,
                                    Precio = x.Precio,
                                    FotoPortada = x.FotoPortada,
                                    Descatalogado = x.Descatalogado,
                                    NombreAutor = x.Autor.Nombre,
                                    NombreEditorial = x.Editorial.Nombre
                                }).ToListAsync();
            return libros;

        }

        public async Task<IEnumerable<LibroDTO>> GetLibrosOrdenadosPorTitulo(bool ascendente)
        {
            IQueryable<Libro> query = _context.Libros.Include(x => x.Autor).Include(x => x.Editorial);

            if (ascendente)
            {
                query = query.OrderBy(x => x.Titulo);
            }
            else
            {
                query = query.OrderByDescending(x => x.Titulo);
            }

            var libros = await query
                .Select(x => new LibroDTO
                {
                    Isbn = x.IdLibro,
                    Titulo = x.Titulo,
                    Paginas = x.Paginas,
                    Precio = x.Precio,
                    FotoPortada = x.FotoPortada,
                    Descatalogado = x.Descatalogado,
                    NombreAutor = x.Autor.Nombre,
                    NombreEditorial = x.Editorial.Nombre
                })
                .ToListAsync();

            return libros;
        }
        public async Task<IEnumerable<LibroDTO>> GetLibrosPorTituloContiene(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                return new List<LibroDTO>();
            }

            var libros = await (from x in _context.Libros
                                where x.Titulo.Contains(texto)  
                                select new LibroDTO
                                {
                                    Isbn = x.IdLibro,
                                    Titulo = x.Titulo,
                                    Paginas = x.Paginas,
                                    Precio = x.Precio,
                                    FotoPortada = x.FotoPortada,
                                    Descatalogado = x.Descatalogado,
                                    NombreAutor = x.Autor.Nombre,
                                    NombreEditorial = x.Editorial.Nombre
                                }).ToListAsync();

            return libros;
        }

        public async Task Add(LibroInsertDTO libroInsertDTO)
        {
            var libro = new Libro
            {
                Titulo = libroInsertDTO.Titulo,
                Paginas = libroInsertDTO.Paginas,
                Precio = libroInsertDTO.Precio,
                Descatalogado = libroInsertDTO.Descatalogado,
                FotoPortada = "",
                EditorialId = (int)libroInsertDTO.EditorialId,
                AutorId = (int)libroInsertDTO.AutorId,
            };

            if (libroInsertDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await libroInsertDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(libroInsertDTO.Foto.FileName);
                    libro.FotoPortada = await _gestorArchivos.GuardarArchivo(contenido, extension, "img", libroInsertDTO.Foto.ContentType);
                }
            }

            await _context.AddAsync(libro);
            await _context.SaveChangesAsync();
        }

        public async Task Update(LibroUpdateDTO libroUpdateDTO)
        {

            var libro = await _context.Libros
                .AsTracking()
                .FirstOrDefaultAsync(e => e.IdLibro == libroUpdateDTO.IdLibro);

            if (libro == null)
            {
                throw new KeyNotFoundException("El libro no fue encontrado");
            }

            libro.Titulo = libroUpdateDTO.Titulo;
            libro.Paginas = libroUpdateDTO.Paginas;
            libro.Precio = libroUpdateDTO.Precio;
            libro.Descatalogado = libroUpdateDTO.Descatalogado;
            libro.EditorialId = (int)libroUpdateDTO.EditorialId;
            libro.AutorId = (int)libroUpdateDTO.AutorId;
            
            if (libroUpdateDTO.Foto != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await libroUpdateDTO.Foto.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(libroUpdateDTO.Foto.FileName);
                    libro.FotoPortada = await _gestorArchivos.GuardarArchivo(contenido, extension, "img", libroUpdateDTO.Foto.ContentType);
                }
            }

            await _context.SaveChangesAsync();
        }
        public void Delete(Libro libro) =>
           _context.Libros.Remove(libro);

        public async Task<Libro> GetLibroPorId(int id)
        {
            return await _context.Libros.FindAsync(id);
        }
        

        public async Task DeleteLibro(LibroDTO libroDTO)
        {
            var libro = await _context.Libros.FindAsync(libroDTO.Isbn);
            if (libro == null)
            {
                throw new KeyNotFoundException("El libro no fue encontrado");
            }
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

        public async Task<IEnumerable<Libro>> GetLibros()
        {
            return await _context.Libros.ToListAsync();
        }

    }
}

