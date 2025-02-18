﻿using AutoMapper;
using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Repository;

namespace Biblioteca.Services
{
    public class EditorialService : ICommonService<EditorialDTO, EditorialInsertDTO, EditorialUpdateDTO>
    {
        private IRepository<Editorial> _editorialRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }

        public EditorialService(IRepository<Editorial> editorialRepository,
            IMapper mapper)
        {
            _editorialRepository = editorialRepository;
            _mapper = mapper;
            Errors = new List<string>();

        }

        public async Task<IEnumerable<EditorialDTO>> Get()
        {
            var editorials = await _editorialRepository.Get();
            return editorials.Select(editorial => _mapper.Map<EditorialDTO>(editorial));
        }

        public async Task<EditorialDTO> GetById(int id)
        {
            var editorial = await _editorialRepository.GetById(id);

            if (editorial != null)
            {
                var editorialDTO = _mapper.Map<EditorialDTO>(editorial);
                return editorialDTO;
            }

            return null;
        }

        public async Task<EditorialDTO> Add(EditorialInsertDTO editorialInsertDTO)
        {
            var editorial = _mapper.Map<Editorial>(editorialInsertDTO);

            await _editorialRepository.Add(editorial);
            await _editorialRepository.Save();

            var editorialDTO = _mapper.Map<EditorialDTO>(editorial);

            return editorialDTO;
        }
        public async Task<EditorialDTO> Update(int id, EditorialUpdateDTO editorialUpdateDTO)
        {
            var editorial = await _editorialRepository.GetById(id);

            if (editorial != null)
            {
                editorial = _mapper.Map<EditorialUpdateDTO, Editorial>(editorialUpdateDTO, editorial);

                _editorialRepository.Update(editorial);
                await _editorialRepository.Save();

                var editorialDTO = _mapper.Map<EditorialDTO>(editorial);

                return editorialDTO;
            }
            return null;
        }

        public async Task<EditorialDTO> Delete(int id)
        {
            var editorial = await _editorialRepository.GetById(id);

            if (editorial != null)
            {
                var editorialDTO = _mapper.Map<EditorialDTO>(editorial);

                _editorialRepository.Delete(editorial);
                await _editorialRepository.Save();

                return editorialDTO;
            }
            return null;
        }
        public bool Validate(EditorialInsertDTO editorialInsertDTO)
        {
            if (_editorialRepository.Search(b => b.Nombre == editorialInsertDTO.Nombre).Count() > 0)
            {
                Errors.Add("Ya existe un editorial con ese nombre");
                return false;
            }
            return true;
        }

        public bool Validate(EditorialUpdateDTO editorialUpdateDTO)
        {
            if (_editorialRepository.Search(b => b.Nombre == editorialUpdateDTO.Nombre && editorialUpdateDTO.IdEditorial !=
            b.IdEditorial).Count() > 0)
            {
                Errors.Add("Ya existe un editorial con ese nombre");
                return false;
            }
            return true;
        }
    }
}

