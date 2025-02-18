using Biblioteca.DTOs;
using Biblioteca.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorialesController : ControllerBase
    {
        private IValidator<EditorialInsertDTO> _editorialInsertValidator;
        private IValidator<EditorialUpdateDTO> _editorialUpdateValidator;
        private ICommonService<EditorialDTO, EditorialInsertDTO, EditorialUpdateDTO> _editorialService;

        public EditorialesController(IValidator<EditorialInsertDTO> editorialInsertValidator,
            IValidator<EditorialUpdateDTO> editorialUpdateValidator,
            [FromKeyedServices("editorialService")]ICommonService<EditorialDTO, EditorialInsertDTO, EditorialUpdateDTO>
            editorialService)
        {
            _editorialInsertValidator = editorialInsertValidator;
            _editorialUpdateValidator = editorialUpdateValidator;
            _editorialService = editorialService;
        }

        [HttpGet]
        public async Task<IEnumerable<EditorialDTO>> Get() =>
            await _editorialService.Get();

        [HttpGet("{id}")]
        public async Task<ActionResult<EditorialDTO>> GetById(int id)
        {
            var editorialDTO = await _editorialService.GetById(id);
            return editorialDTO == null ? NotFound() : Ok(editorialDTO);
        }

        [HttpPost]
        public async Task<ActionResult<EditorialDTO>> Add(EditorialInsertDTO editorialInsertDTO)
        {
            var validationResult = await _editorialInsertValidator.ValidateAsync(editorialInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_editorialService.Validate(editorialInsertDTO))
            {
                return BadRequest(_editorialService.Errors);
            }

            var editorialDTO = await _editorialService.Add(editorialInsertDTO);

            return CreatedAtAction(nameof(GetById), new { id = editorialDTO.IdEditorial }, editorialDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EditorialDTO>> Update(int id, EditorialUpdateDTO editorialUpdateDTO)
        {
            var validationResult = await _editorialUpdateValidator.ValidateAsync(editorialUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_editorialService.Validate(editorialUpdateDTO))
            {
                return BadRequest(_editorialService.Errors);
            }

            var editorialDTO = await _editorialService.Update(id, editorialUpdateDTO);

            return editorialDTO == null ? NotFound() : Ok(editorialDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<EditorialDTO>> Delete(int id)
        {
            var editorialDTO = await _editorialService.Delete(id);
            return editorialDTO == null ? NotFound() : Ok(editorialDTO);
        }
    }
}
