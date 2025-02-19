using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperacionesController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public OperacionesController(BibliotecaContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetOperaciones()
        {
            var operaciones = await _context.Operaciones.ToListAsync();
            return Ok(operaciones);
        }

    }
}

