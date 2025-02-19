using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("ordenadosNombre/{ascen}")]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditorialesOrdenadasPorNombre(bool ascen)
        {
            await _operacionesService.AddOperacion("Obtener editoriales ordenadas por su nombre", "Editoriales");
            var editoriales = await _editorialService.GetEditorialesOrdenadasPorNombre(ascen);

            if (!editoriales.Any())
            {
                return NotFound("No se encontraron editoriales");
            }

            return Ok(editoriales);
        }

        [HttpGet("nombre/contiene/{texto}")]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditorialesPorNombreContiene(string texto)
        {
            await _operacionesService.AddOperacion("Obtener editoriales con el nombre que contiene", "Editoriales");
            if (string.IsNullOrEmpty(texto))
            {
                return BadRequest("El texto de búsqueda no puede estar vacío.");
            }

            var editoriales = await _editorialService.GetEditorialesPorNombreContiene(texto);

            if (!editoriales.Any())
            {
                return NotFound("No se encontraron editoriales que contengan el texto especificado.");
            }

            return Ok(editoriales);
        }

        [HttpGet("paginacion/{desde}/{hasta}")]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditorialesPaginados(int desde, int hasta)
        {
            await _operacionesService.AddOperacion("Obtener editoriales paginadas", "Editoriales");
            if (hasta < desde)
            {
                return BadRequest("El máximo no puede ser inferior al mínimo");
            }

            var editoriales = await _editorialService.GetEditorialesPaginadas(desde, hasta);
            return Ok(editoriales);
        }

        [Authorize]
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

        [Authorize]
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

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EditorialDTO>> Delete(int id)
        {
            var editorialDTO = await _editorialService.Delete(id);
            return editorialDTO == null ? NotFound() : Ok(editorialDTO);
        }

    }
}
