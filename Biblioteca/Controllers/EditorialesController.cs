using Biblioteca.DTOs;
using Biblioteca.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorialesController : ControllerBase
    {
        private IValidator<EditorialInsertDTO> _editorialInsertValidator;
        private IValidator<EditorialUpdateDTO> _editorialUpdateValidator;
        private IEditorialService _editorialService;
        private readonly OperacionesService _operacionesService;

        public EditorialesController(IValidator<EditorialInsertDTO> editorialInsertValidator,
            IValidator<EditorialUpdateDTO> editorialUpdateValidator,
            IEditorialService editorialService, 
            OperacionesService operacionesService)
        {
            _editorialInsertValidator = editorialInsertValidator;
            _editorialUpdateValidator = editorialUpdateValidator;
            _editorialService = editorialService;
            _operacionesService = operacionesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EditorialDTO>>> Get()
        {
            await _operacionesService.AddOperacion("Obtener editoriales", "Editoriales");
            var editoriales = await _editorialService.Get();
            return Ok(editoriales);
        }
            
        [HttpGet("{id:int}")]
        public async Task<ActionResult<EditorialDTO>> GetById(int id)
        {
            await _operacionesService.AddOperacion("Obtener editorial por Id", "Editoriales");
            var editorialDTO = await _editorialService.GetById(id);
            return editorialDTO == null ? NotFound() : Ok(editorialDTO);
        }

        [HttpGet("editorialesLibros/{id:int}")]
        public async Task<ActionResult<EditorialLibroDTO>> GetEditorialesLibrosEager(int id)
        {
            await _operacionesService.AddOperacion("Obtener editorial con todos sus libros", "Editoriales");
            var editorial = await _editorialService.GetEditorialesLibrosEager(id);

            if (editorial == null)
            {
                return NotFound();
            }

            return Ok(editorial);
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

        [HttpPut("{id:int}")]
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EditorialDTO>> Delete(int id)
        {
            var editorialDTO = await _editorialService.Delete(id);
            return editorialDTO == null ? NotFound() : Ok(editorialDTO);
        }

    }
}
