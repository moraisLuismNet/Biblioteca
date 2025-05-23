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
    public class LibrosController : ControllerBase
    {
        private IValidator<LibroInsertDTO> _libroInsertValidator;
        private IValidator<LibroUpdateDTO> _libroUpdateValidator;
        private ILibroService _libroService;
        private readonly OperacionesService _operacionesService;

        public LibrosController(IValidator<LibroInsertDTO> libroInsertValidator,
            IValidator<LibroUpdateDTO> libroUpdateValidator,
            ILibroService libroService,
            OperacionesService operacionesService)
        {
            _libroInsertValidator = libroInsertValidator;
            _libroUpdateValidator = libroUpdateValidator;
            _libroService = libroService;
            _operacionesService = operacionesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> Get()
        {
            await _operacionesService.AddOperacion("Obtener libros", "Libros");
            var libros = await _libroService.Get();
            return Ok(libros);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> GetById(int id)
        {
            await _operacionesService.AddOperacion("Obtener libro por Id", "Libros");
            var libroDTO = await _libroService.GetById(id);
            if (libroDTO == null)
            {
                return NotFound(new { mensaje = "El libro no fue encontrado" });
            }
            return Ok(libroDTO);
        }

        [HttpGet("venta")]
        public async Task<ActionResult<IEnumerable<LibroVentaDTO>>> GetLibrosYPrecios()
        {
            await _operacionesService.AddOperacion("Obtener libros y precios", "Libros");
            var libros = await _libroService.GetLibrosYPrecios();

            return Ok(libros);
        }

        /* Obtener todos los libros agrupados por su propiedad Descatalogado. Por cada grupo, obtener cuántos
        hay descatalogados y cuántos hay no descatalogados */
        [HttpGet("groupByDescatalogado")]
        public async Task<ActionResult<IEnumerable<LibroGroupDTO>>> GetLibrosGroupedByDescatalogado()
        {
            await _operacionesService.AddOperacion("Obtener libros agrupados por su propiedad Descatalogado", "Libros");
            var groupedData = await _libroService.GetLibrosGroupedByDescatalogado();
            return Ok(groupedData);
        }

        [HttpGet("paginacion/{desde}/{hasta}")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibrosPaginados(int desde, int hasta)
        {
            await _operacionesService.AddOperacion("Obtener libros paginados", "Libros");
            if (hasta < desde)
            {
                return BadRequest("El máximo no puede ser inferior al mínimo");
            }

            var libros = await _libroService.GetLibrosPaginados(desde, hasta);
            return Ok(libros);
        }

        [HttpGet("precio/{min}/{max}")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibrosPorPrecio(decimal min, decimal max)
        {
            await _operacionesService.AddOperacion("Obtener libros con un precio determinado", "Libros");
            if (min > max)
            {
                return BadRequest("Precio mínimo no puede ser mayor que el precio máximo");
            }

            var libros = await _libroService.GetLibrosPorPrecio(min, max);

            if (!libros.Any())
            {
                return NotFound("No se encontraron libros en el rango de precios especificado");
            }

            return Ok(libros);
        }

        [HttpGet("ordenadosTitulo/{ascen}")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibrosOrdenadosPorTitulo(bool ascen)
        {
            await _operacionesService.AddOperacion("Obtener libros ordenados por su título", "Libros");
            var libros = await _libroService.GetLibrosOrdenadosPorTitulo(ascen);

            if (!libros.Any())
            {
                return NotFound("No se encontraron libros");
            }

            return Ok(libros);
        }

        [HttpGet("titulo/contiene/{texto}")]
        public async Task<ActionResult<IEnumerable<LibroDTO>>> GetLibrosPorTituloContiene(string texto)
        {
            await _operacionesService.AddOperacion("Obtener libros con el título que contiene", "Libros");
            if (string.IsNullOrEmpty(texto))
            {
                return BadRequest("El texto de búsqueda no puede estar vacío");
            }

            var libros = await _libroService.GetLibrosPorTituloContiene(texto);

            if (!libros.Any())
            {
                return NotFound("No se encontraron libros que contengan el texto especificado");
            }

            return Ok(libros);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<LibroDTO>> Add([FromForm] LibroInsertDTO libroInsertDTO)
        {
            await _operacionesService.AddOperacion("Añadir libro", "Libros");
            if (!_libroService.Validate(libroInsertDTO))
            {
                return BadRequest(_libroService.Errors);
            }

            var newLibro = await _libroService.Add(libroInsertDTO);

            return Ok(newLibro);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] LibroUpdateDTO dtoLibro)
        {
            await _operacionesService.AddOperacion("Aactualizar libro", "Libros");
            if (!_libroService.Validate(dtoLibro))
            {
                return BadRequest(_libroService.Errors);
            }

            var libroActualizado = await _libroService.Update(id, dtoLibro);

            if (libroActualizado == null)
            {
                return NotFound("El libro no fue encontrado o hubo un error");
            }

            return Ok(libroActualizado);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _operacionesService.AddOperacion("Eliminar libro", "Libros");
            var result = await _libroService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}

