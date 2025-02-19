using AutoMapper;
using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Services
{
    public class AutorService : ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>
    {
        private IAutorRepository _autorRepository;
        private IMapper _mapper;
        private BibliotecaContext _context;
        public List<string> Errors { get; }

        public AutorService(IAutorRepository autorRepository,
            IMapper mapper,
            BibliotecaContext context)
        {
            _autorRepository = autorRepository;
            _mapper = mapper;
            Errors = new List<string>();
            _context = context;
        }

        public async Task<IEnumerable<AutorDTO>> Get()
        {
            var autors = await _autorRepository.Get();
            return autors.Select(autor => _mapper.Map<AutorDTO>(autor));
        }

        public async Task<AutorDTO> GetById(int id)
        {
            var autor = await _autorRepository.GetById(id);

            if (autor != null)
            {
                var autorDTO = _mapper.Map<AutorDTO>(autor);
                return autorDTO;
            }

            return null;
        }  

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
                })
                .ToListAsync();
        }

        public async Task<AutorLibroDTO?> GetAutorLibrosSelect(int id)
        {
            return await _autorRepository.GetAutorLibrosSelect(id);
        }

        public async Task<AutorDTO> Add(AutorInsertDTO autorInsertDTO)
        {
            var autor = _mapper.Map<Autor>(autorInsertDTO);

            await _autorRepository.Add(autor);
            await _autorRepository.Save();

            var autorDTO = _mapper.Map<AutorDTO>(autor);

            return autorDTO;
        }
        public async Task<AutorDTO> Update(int id, AutorUpdateDTO autorUpdateDTO)
        {
            var autor = await _autorRepository.GetById(id);

            if (autor != null)
            {
                autor = _mapper.Map<AutorUpdateDTO, Autor>(autorUpdateDTO, autor);

                _autorRepository.Update(autor);
                await _autorRepository.Save();

                var autorDTO = _mapper.Map<AutorDTO>(autor);

                return autorDTO;
            }
            return null;
        }

        public async Task<AutorDTO> Delete(int id)
        {
            var autor = await _autorRepository.GetById(id);

            if (autor != null)
            {
                var autorDTO = _mapper.Map<AutorDTO>(autor);

                _autorRepository.Delete(autor);
                await _autorRepository.Save();

                return autorDTO;
            }
            return null;
        }
        public bool Validate(AutorInsertDTO autorInsertDTO)
        {
            if (_autorRepository.Search(b => b.Nombre == autorInsertDTO.Nombre).Count() > 0)
            {
                Errors.Add("Ya existe un autor con ese nombre");
                return false;
            }
            return true;
        }

        public bool Validate(AutorUpdateDTO autorUpdateDTO)
        {
            if (_autorRepository.Search(b => b.Nombre == autorUpdateDTO.Nombre && autorUpdateDTO.IdAutor !=
            b.IdAutor).Count() > 0)
            {
                Errors.Add("Ya existe un autor con ese nombre");
                return false;
            }
            return true;

        }

        public Task<EditorialLibroDTO> GetEditorialesLibrosEager(int id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<LibroVentaDTO>> ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>.GetLibrosYPrecios()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<LibroGroupDTO>> ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>.GetLibrosGroupedByDescatalogado()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Libro>> GetLibrosPaginados(int desde, int hasta)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Libro>> ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>.GetLibrosPorPrecio(decimal precioMin, decimal precioMax)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Libro>> GetLibrosOrdenadosPorTitulo(bool ascendente)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Libro>> ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>.GetLibrosPorTituloContiene(string texto)
        {
            throw new NotImplementedException();
        }

        public Task<Libro> GetLibroPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task EliminarLibro(Libro libro)
        {
            throw new NotImplementedException();
        }

    }
}
