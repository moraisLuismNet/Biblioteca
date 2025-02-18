using AutoMapper;
using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Repository;

namespace Biblioteca.Services
{
    public class LibroService : ICommonService<LibroDTO, LibroInsertDTO, LibroUpdateDTO>
    {
        private IRepository<Libro> _libroRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }

        public LibroService(IRepository<Libro> libroRepository,
            IMapper mapper)
        {
            _libroRepository = libroRepository;
            _mapper = mapper;
            Errors = new List<string>();

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

        public async Task<LibroDTO> Add(LibroInsertDTO libroInsertDTO)
        {
            var libro = _mapper.Map<Libro>(libroInsertDTO);

            await _libroRepository.Add(libro);
            await _libroRepository.Save();

            var libroDTO = _mapper.Map<LibroDTO>(libro);

            return libroDTO;
        }
        public async Task<LibroDTO> Update(int id, LibroUpdateDTO libroUpdateDTO)
        {
            var libro = await _libroRepository.GetById(id);

            if (libro != null)
            {
                libro = _mapper.Map<LibroUpdateDTO, Libro>(libroUpdateDTO, libro);

                _libroRepository.Update(libro);
                await _libroRepository.Save();

                var libroDTO = _mapper.Map<LibroDTO>(libro);

                return libroDTO;
            }
            return null;
        }

        public async Task<LibroDTO> Delete(int id)
        {
            var libro = await _libroRepository.GetById(id);

            if (libro != null)
            {
                var libroDTO = _mapper.Map<LibroDTO>(libro);

                _libroRepository.Delete(libro);
                await _libroRepository.Save();

                return libroDTO;
            }
            return null;
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

        public bool Validate(LibroUpdateDTO libroUpdateDTO)
        {
            if (_libroRepository.Search(b => b.Titulo == libroUpdateDTO.Titulo && libroUpdateDTO.IdLibro !=
            b.IdLibro).Count() > 0)
            {
                Errors.Add("Ya existe un libro con ese nombre");
                return false;
            }
            return true;

        }
    }
}

