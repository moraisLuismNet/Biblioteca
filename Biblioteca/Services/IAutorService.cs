using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IAutorService : ICommonServiceBase<AutorDTO, AutorInsertDTO, AutorUpdateDTO>
    {
        Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles();
        Task<AutorLibroDTO> GetAutorLibrosSelect(int id);
        Task<IEnumerable<Autor>> GetAutoresOrdenadosPorNombre(bool ascendente);
        Task<IEnumerable<Autor>> GetAutoresPorNombreContiene(string texto);
        Task<IEnumerable<Autor>> GetAutoresPaginados(int desde, int hasta);
    }
}