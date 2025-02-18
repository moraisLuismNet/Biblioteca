using Biblioteca.DTOs;
using Biblioteca.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoresController : ControllerBase
    {
        private IValidator<AutorInsertDTO> _autorInsertValidator;
        private IValidator<AutorUpdateDTO> _autorUpdateValidator;
        private ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO> _autorService;
        private readonly OperacionesService _operacionesService;

        public AutoresController(IValidator<AutorInsertDTO> autorInsertValidator,
            IValidator<AutorUpdateDTO> autorUpdateValidator,
            [FromKeyedServices("autorService")]ICommonService<AutorDTO, AutorInsertDTO, AutorUpdateDTO>
            autorService, OperacionesService operacionesService)
        {
            _autorInsertValidator = autorInsertValidator;
            _autorUpdateValidator = autorUpdateValidator;
            _autorService = autorService;
            _operacionesService = operacionesService;
        }

        [HttpGet]
        public async Task<IEnumerable<AutorDTO>> Get() =>
            await _autorService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDTO>> GetById(int id)
        {
            var autorDTO = await _autorService.GetById(id);
            return autorDTO == null ? NotFound() : Ok(autorDTO);
        }

        [HttpPost]
        public async Task<ActionResult<AutorDTO>> Add(AutorInsertDTO autorInsertDTO)
        {
            var validationResult = await _autorInsertValidator.ValidateAsync(autorInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_autorService.Validate(autorInsertDTO))
            {
                return BadRequest(_autorService.Errors);
            }

            var autorDTO = await _autorService.Add(autorInsertDTO);

            return CreatedAtAction(nameof(GetById), new { id = autorDTO.IdAutor }, autorDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AutorDTO>> Update(int id, AutorUpdateDTO autorUpdateDTO)
        {
            var validationResult = await _autorUpdateValidator.ValidateAsync(autorUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_autorService.Validate(autorUpdateDTO))
            {
                return BadRequest(_autorService.Errors);
            }

            var autorDTO = await _autorService.Update(id, autorUpdateDTO);

            return autorDTO == null ? NotFound() : Ok(autorDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AutorDTO>> Delete(int id)
        {
            var autorDTO = await _autorService.Delete(id);
            await _operacionesService.AddOperacion("Delete", "Autores");
            return autorDTO == null ? NotFound($"Autor con ID {id} no encontrado") : Ok(autorDTO);
        }
    }
}

