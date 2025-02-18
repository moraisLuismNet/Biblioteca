using AutoMapper;
using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Repository;

namespace Biblioteca.Services
{
    public class AutorService : ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>
    {
        private IRepository<Autor> _autorRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }

        public AutorService(IRepository<Autor> autorRepository,
            IMapper mapper)
        {
            _autorRepository = autorRepository;
            _mapper = mapper;
            Errors = new List<string>();

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
    }
}
