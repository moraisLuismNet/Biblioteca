using AutoMapper;
using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    public class LibroService : ICommonService<LibroDTO, LibroInsertDTO, LibroUpdateDTO>
    {
        private IRepository<Libro> _libroRepository;
        private IMapper _mapper;
        private readonly IGestorArchivos _gestorArchivos;
        public List<string> Errors { get; }

        public LibroService(IRepository<Libro> libroRepository,
            IMapper mapper, IGestorArchivos gestorArchivos)
        {
            _libroRepository = libroRepository;
            _mapper = mapper;
            Errors = new List<string>();
            _gestorArchivos = gestorArchivos;
        }

        public async Task<IEnumerable<LibroDTO>> Get()
        {
            var libros = await _libroRepository.Get();
            return libros.Select(libro => _mapper.Map<LibroDTO>(libro));
        }

        public async Task<LibroDTO> GetById(int id)
        {
            var libro = await _libroRepository.GetById(id);

            if (libro != null)
            {
                var libroDTO = _mapper.Map<LibroDTO>(libro);
                return libroDTO;
            }

            return null;
        }

        public async Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios()
        {
            return await _libroRepository.GetLibrosYPrecios();
        }

        public async Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado()
        {
            return await _libroRepository.GetLibrosGroupedByDescatalogado();
        }

        public async Task<IEnumerable<Libro>> GetLibrosPaginados(int desde, int hasta)
        {
            return await _libroRepository.GetLibrosPaginados(desde, hasta);
        }

        public async Task<IEnumerable<Libro>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax)
        {
            return await _libroRepository.GetLibrosPorPrecio(precioMin, precioMax);
        }

        public async Task<IEnumerable<Libro>> GetLibrosOrdenadosPorTitulo(bool ascendente)
        {
            return await _libroRepository.GetLibrosOrdenadosPorTitulo(ascendente);
        }

        public async Task<IEnumerable<Libro>> GetLibrosPorTituloContiene(string texto)
        {
            return await _libroRepository.GetLibrosPorTituloContiene(texto);
        }

        public async Task<LibroDTO> Add(LibroInsertDTO libroInsertDTO)
        {
            var libro = _mapper.Map<Libro>(libroInsertDTO);

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

            await _libroRepository.Add(libro);
            await _libroRepository.Save();

            return _mapper.Map<LibroDTO>(libro);
        }
        
        public async Task<LibroDTO> Update(int id, LibroUpdateDTO libroUpdateDTO)
        {
            var libro = await _libroRepository.GetById(id);
            


            if (libro == null)
            {
                Errors.Add($"Libro con ISBN {id} no encontrado.");
                return null;
            }

            if (!await _libroRepository.ExisteAutor(libroUpdateDTO.AutorId) || !await _libroRepository.ExisteEditorial(libroUpdateDTO.EditorialId))
            {
                Errors.Add("Autor o Editorial no existe.");
                return null;
            }

            libro.Titulo = libroUpdateDTO.Titulo;
            libro.Paginas = libroUpdateDTO.Paginas;
            libro.Precio = libroUpdateDTO.Precio;
            libro.Descatalogado = libroUpdateDTO.Descatalogado;
            libro.AutorId = libroUpdateDTO.AutorId;
            libro.EditorialId = libroUpdateDTO.EditorialId;

            if (libroUpdateDTO.Foto != null)
            {
                await UpdateFotoPortada(libro, libroUpdateDTO.Foto);
                
            }
            else if (!string.IsNullOrEmpty(libro.FotoPortada))
            {
                await _gestorArchivos.BorrarArchivo(libro.FotoPortada, "img");
                libro.FotoPortada = null;
            }

            try
            {
                _libroRepository.Update(libro);
                await _libroRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _libroRepository.LibroExists(id))
                {
                    Errors.Add($"Libro con ISBN {id} no encontrado.");
                    return null;
                }
                throw;
            }
            return _mapper.Map<LibroDTO>(libro);
        }

        public bool Validate(LibroUpdateDTO libroUpdateDTO)
        {
            if (_libroRepository.Search(l => l.Titulo == libroUpdateDTO.Titulo && l.IdLibro != libroUpdateDTO.IdLibro).Any())
            {
                Errors.Add("Ya existe un libro con ese título.");
                return false;
            }
            return true;
        }

        private async Task UpdateFotoPortada(Libro libro, IFormFile foto)
        {
            if (!string.IsNullOrEmpty(libro.FotoPortada))
            {
                await _gestorArchivos.BorrarArchivo(libro.FotoPortada, "img");
            }

            using (var memoryStream = new MemoryStream())
            {
                await foto.CopyToAsync(memoryStream);
                var contenido = memoryStream.ToArray();
                var extension = Path.GetExtension(foto.FileName);
                libro.FotoPortada = await _gestorArchivos.GuardarArchivo(contenido, extension, "img", foto.ContentType);
            }
        }
    
        public async Task<Libro> GetLibroPorId(int id)
        {
            return await _libroRepository.GetLibroPorId(id);
        }

        public async Task EliminarLibro(Libro libro)
        {
            await _libroRepository.DeleteLibro(libro);
        }

        public bool Validate(LibroInsertDTO libroInsertDTO)
        {
            if (_libroRepository.Search(b => b.Titulo == libroInsertDTO.Titulo).Count() > 0)
            {
                Errors.Add("Ya existe un libro con ese titulo");
                return false;
            }
            return true;
        }

        public Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles()
        {
            throw new NotImplementedException();
        }

        public Task GetAutorLibrosSelect(int id)
        {
            throw new NotImplementedException();
        }

        Task<AutorLibroDTO> ICommonService<LibroDTO, LibroInsertDTO, LibroUpdateDTO>.GetAutorLibrosSelect(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<EditorialLibroDTO> GetEditorialesLibrosEager(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<LibroDTO?> Delete(int id)
        {
            var libro = await _libroRepository.GetById(id);
            if (libro == null)
            {
                return null;
            }

            await _gestorArchivos.BorrarArchivo(libro.FotoPortada, "img");

            await _libroRepository.DeleteLibro(libro);

            return _mapper.Map<LibroDTO>(libro);
        }

    }

}

