using Biblioteca.Models;

namespace Biblioteca.Services
{
    public class OperacionesService
    {
        private readonly BibliotecaContext _context;
        private readonly IHttpContextAccessor _accessor;

        public OperacionesService(BibliotecaContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public async Task<bool> AddOperacion(string operacion, string controller)
        {
            try
            {
                var nuevaOperacion = new Operacion
                {
                    FechaAccion = DateTime.Now,
                    Operation = operacion,
                    Controller = controller,
                    Ip = _accessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                };

                await _context.Operaciones.AddAsync(nuevaOperacion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
