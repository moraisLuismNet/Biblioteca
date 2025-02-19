using Biblioteca.DTOs;

namespace Biblioteca.Services
{
    public interface ITokenService
    {
        LoginResponseDTO GenerarToken(UsuarioDTO credencialesUsuario);
    }

}
