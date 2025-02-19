using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IAutorService : ICommonServiceBase<AutorDTO, AutorInsertDTO, AutorUpdateDTO>
    {
        Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles();
        Task<AutorLibroDTO> GetAutorLibrosSelect(int id);
    }
}