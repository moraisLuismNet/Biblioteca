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

        [HttpGet("conTotalLibros")]
        public async Task<ActionResult> Get()
        {
            await _operacionesService.AddOperacion("Obtener editoriales con total libros", "Editoriales");
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
        public async Task<ActionResult<EditorialLibroDTO>> GetEditorialLibrosSelect(int id)
        {
            await _operacionesService.AddOperacion("Obtener editorial con todos sus libros", "Editoriales");
            var editorial = await _editorialService.GetEditorialLibrosSelect(id);

            if (editorial == null)
            {
                return NotFound();
            }

            return Ok(editorial);
        }

        [HttpGet("ordenadosNombre/{ascen}")]
        public async Task<ActionResult<IEnumerable<EditorialInsertDTO>>> GetEditorialesOrdenadasPorNombre(bool ascen)
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
        public async Task<ActionResult<IEnumerable<EditorialInsertDTO>>> GetEditorialesPorNombreContiene(string texto)
        {
            await _operacionesService.AddOperacion("Obtener editoriales con el nombre que contiene", "Editoriales");
            if (string.IsNullOrEmpty(texto))
            {
                return BadRequest("El texto de búsqueda no puede estar vacío");
            }

            var editoriales = await _editorialService.GetEditorialesPorNombreContiene(texto);

            if (!editoriales.Any())
            {
                return NotFound("No se encontraron editoriales que contengan el texto especificado");
            }

            return Ok(editoriales);
        }

        [HttpGet("paginacion/{desde}/{hasta}")]
        public async Task<ActionResult<IEnumerable<EditorialInsertDTO>>> GetEditorialesPaginados(int desde, int hasta)
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
        public async Task<ActionResult<EditorialInsertDTO>> Add([FromBody] EditorialInsertDTO editorial)
        {
            await _operacionesService.AddOperacion("Añadir editorial", "Editoriales");
            var validationResult = await _editorialInsertValidator.ValidateAsync(editorial);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var newEditorial = await _editorialService.Add(editorial);
            return CreatedAtAction(nameof(Get), new { id = newEditorial.NombreEditorial }, newEditorial);
        }

        [Authorize]
        [HttpPut("{idEditorial:int}")]
        public async Task<IActionResult> Update(int idEditorial, [FromBody] EditorialUpdateDTO editorial)
        {
            await _operacionesService.AddOperacion("Actualizar editorial", "Editoriales");
            var validationResult = await _editorialUpdateValidator.ValidateAsync(editorial);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            if (idEditorial != editorial.IdEditorial)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del cuerpo" });
            }

            var editorialActualizada = await _editorialService.Update(idEditorial, editorial);

            if (editorialActualizada != null)
            {
                return Ok(new { message = "La editorial ha sido actualizada exitosamente" }); 
            }

            return NotFound(new { message = "La editorial no fue encontrada" });
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<EditorialDTO>> Delete(int id)
        {
            await _operacionesService.AddOperacion("Eliminar editorial", "Editoriales");
            var editorialDTO = await _editorialService.Delete(id);
            return editorialDTO == null ? NotFound() : Ok(editorialDTO);
        }

    }
}
