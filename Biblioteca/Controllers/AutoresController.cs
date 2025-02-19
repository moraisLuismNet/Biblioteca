using Biblioteca.DTOs;
using Biblioteca.Models;
using Biblioteca.Services;
using FluentValidation;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            await _operacionesService.AddOperacion("Obtener autores", "Autores");
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

            if (autoresDto == null || !autoresDto.Any()) // Validación para evitar errores en la respuesta
            {
                return NotFound("No se encontraron autores con detalles.");
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
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutoresOrdenadosPorNombre(bool ascen)
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
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutoresPorNombreContiene(string texto)
        {
            await _operacionesService.AddOperacion("Obtener autores con el nombre que contiene", "Autores");
            if (string.IsNullOrEmpty(texto))
            {
                return BadRequest("El texto de búsqueda no puede estar vacío.");
            }

            var autores = await _autorService.GetAutoresPorNombreContiene(texto);

            if (!autores.Any())
            {
                return NotFound("No se encontraron autores que contengan el texto especificado.");
            }

            return Ok(autores);
        }

        [HttpGet("paginacion/{desde}/{hasta}")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutoresPaginados(int desde, int hasta)
        {
            await _operacionesService.AddOperacion("Obtener autores paginados", "Autores");
            if (hasta < desde)
            {
                return BadRequest("El máximo no puede ser inferior al mínimo");
            }

            var autores = await _autorService.GetAutoresPaginados(desde, hasta);
            return Ok(autores);
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

        [HttpPut("{id:int}")]
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<AutorDTO>> Delete(int id)
        {
            var autorDTO = await _autorService.Delete(id);
            return autorDTO == null ? NotFound($"Autor con ID {id} no encontrado") : Ok(autorDTO);
        }
    }
}

