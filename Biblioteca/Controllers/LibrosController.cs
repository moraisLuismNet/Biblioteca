using Biblioteca.DTOs;
using Biblioteca.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private IValidator<LibroInsertDTO> _libroInsertValidator;
        private IValidator<LibroUpdateDTO> _libroUpdateValidator;
        private ICommonService<LibroDTO, LibroInsertDTO, LibroUpdateDTO> _libroService;

        public LibrosController(IValidator<LibroInsertDTO> libroInsertValidator,
            IValidator<LibroUpdateDTO> libroUpdateValidator,
            [FromKeyedServices("libroService")]ICommonService<LibroDTO, LibroInsertDTO, LibroUpdateDTO>
            libroService)
        {
            _libroInsertValidator = libroInsertValidator;
            _libroUpdateValidator = libroUpdateValidator;
            _libroService = libroService;
        }

        [HttpGet]
        public async Task<IEnumerable<LibroDTO>> Get() =>
            await _libroService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<LibroDTO>> GetById(int id)
        {
            var libroDTO = await _libroService.GetById(id);
            return libroDTO == null ? NotFound() : Ok(libroDTO);
        }

        [HttpPost]
        public async Task<ActionResult<LibroDTO>> Add(LibroInsertDTO libroInsertDTO)
        {
            var validationResult = await _libroInsertValidator.ValidateAsync(libroInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_libroService.Validate(libroInsertDTO))
            {
                return BadRequest(_libroService.Errors);
            }

            var libroDTO = await _libroService.Add(libroInsertDTO);

            return CreatedAtAction(nameof(GetById), new { id = libroDTO.IdLibro }, libroDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LibroDTO>> Update(int id, LibroUpdateDTO libroUpdateDTO)
        {
            var validationResult = await _libroUpdateValidator.ValidateAsync(libroUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_libroService.Validate(libroUpdateDTO))
            {
                return BadRequest(_libroService.Errors);
            }

            var libroDTO = await _libroService.Update(id, libroUpdateDTO);

            return libroDTO == null ? NotFound() : Ok(libroDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<LibroDTO>> Delete(int id)
        {
            var libroDTO = await _libroService.Delete(id);
            return libroDTO == null ? NotFound() : Ok(libroDTO);
        }
    }
}

