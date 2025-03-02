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
    public class AutoresController : ControllerBase
    {
        private IValidator<AutorInsertDTO> _autorInsertValidator;
        private IValidator<AutorUpdateDTO> _autorUpdateValidator;
        private IAutorService _autorService;
        private readonly OperacionesService _operacionesService;
        public AutoresController(
            IValidator<AutorInsertDTO> autorInsertValidator,
            IValidator<AutorUpdateDTO> autorUpdateValidator,
            IAutorService autorService,
            OperacionesService operacionesService)
        {
            _autorInsertValidator = autorInsertValidator;
            _autorUpdateValidator = autorUpdateValidator;
            _autorService = autorService;
            _operacionesService = operacionesService;
        }

        [HttpGet("conTotalLibros")]
        public async Task<ActionResult> Get()
        {
            await _operacionesService.AddOperacion("Obtener autores con total libros", "Autores");
            var autores = await _autorService.Get();
            return Ok(autores);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDTO>> GetById(int id)
        {
            await _operacionesService.AddOperacion("Obtener autor por Id", "Autores");
            var autorDTO = await _autorService.GetById(id);
            return autorDTO == null ? NotFound() : Ok(autorDTO);
        }

        [HttpGet("detalles")]
        public async Task<ActionResult<IEnumerable<AutorLibroDTO>>> GetAutoresConDetalles()
        {
            await _operacionesService.AddOperacion("Obtener autores con detalles", "Autores");
            var autoresDto = await _autorService.GetAutoresConDetalles();

            if (autoresDto == null || !autoresDto.Any()) 
            {
                return NotFound("No se encontraron autores con detalles");
            }

            return Ok(autoresDto);
        }

        [HttpGet("autorLibrosSelect")]
        public async Task<ActionResult<AutorLibroDTO>> GetAutoresLibrosSelect(int id)
        {
            await _operacionesService.AddOperacion("Obtener autor con detalles de sus libros", "Autores");
            var autor = await _autorService.GetAutorLibrosSelect(id);

            if (autor == null)
            {
                return NotFound();
            }

            return Ok(autor);
        }

        [HttpGet("ordenadosNombre/{ascen}")]
        public async Task<ActionResult<IEnumerable<AutorInsertDTO>>> GetAutoresOrdenadosPorNombre(bool ascen)
        {
            await _operacionesService.AddOperacion("Obtener autores ordenados por su nombre", "Autores");
            var autores = await _autorService.GetAutoresOrdenadosPorNombre(ascen);

            if (!autores.Any())
            {
                return NotFound("No se encontraron autores");
            }

            return Ok(autores);
        }

        [HttpGet("nombre/contiene/{texto}")]
        public async Task<ActionResult<IEnumerable<AutorInsertDTO>>> GetAutoresPorNombreContiene(string texto)
        {
            await _operacionesService.AddOperacion("Obtener autores con el nombre que contiene", "Autores");
            if (string.IsNullOrEmpty(texto))
            {
                return BadRequest("El texto de búsqueda no puede estar vacío");
            }

            var autores = await _autorService.GetAutoresPorNombreContiene(texto);

            if (!autores.Any())
            {
                return NotFound("No se encontraron autores que contengan el texto especificado");
            }

            return Ok(autores);
        }

        [HttpGet("paginacion/{desde}/{hasta}")]
        public async Task<ActionResult<IEnumerable<AutorInsertDTO>>> GetAutoresPaginados(int desde, int hasta)
        {
            await _operacionesService.AddOperacion("Obtener autores paginados", "Autores");
            if (hasta < desde)
            {
                return BadRequest("El máximo no puede ser inferior al mínimo");
            }

            var autores = await _autorService.GetAutoresPaginados(desde, hasta);
            return Ok(autores);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AutorInsertDTO>> Add([FromBody] AutorInsertDTO autor)
        {
            await _operacionesService.AddOperacion("Añadir autor", "Autores");
            var validationResult = await _autorInsertValidator.ValidateAsync(autor);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var newAutor = await _autorService.Add(autor);
            return CreatedAtAction(nameof(Get), new { id = newAutor.NombreAutor }, newAutor);
        }

        [Authorize]
        [HttpPut("{idAutor:int}")]
        public async Task<IActionResult> Update(int idAutor, [FromBody] AutorUpdateDTO autor)
        {
            await _operacionesService.AddOperacion("Actualizar autor", "Autores");
            var validationResult = await _autorUpdateValidator.ValidateAsync(autor);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idAutor != autor.IdAutor)
            {
                return BadRequest(new { message = "El ID de la ruta no coincide con el ID del cuerpo" });
            }

            var autorActualizado = await _autorService.Update(idAutor, autor);

            if (autorActualizado != null)
            {
                return Ok(new { message = "El autor ha sido actualizado exitosamente" });
            }

            return NotFound(new { message = "El autor no fue encontrado" });
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<AutorDTO>> Delete(int id)
        {
            await _operacionesService.AddOperacion("Eliminar autor", "Autores");
            var autorDTO = await _autorService.Delete(id);
            return autorDTO == null ? NotFound($"Autor con ID {id} no encontrado") : Ok(autorDTO);
        }
    }
}

