using Biblioteca.DTOs;

namespace Biblioteca.Services
{
    public interface IAutorService : ICommonServiceBase<AutorDTO, AutorInsertDTO, AutorUpdateDTO>
    {
        Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles();
        Task<AutorLibroDTO> GetAutorLibrosSelect(int id);
        Task<IEnumerable<AutorInsertDTO>> GetAutoresOrdenadosPorNombre(bool ascendente);
        Task<IEnumerable<AutorInsertDTO>> GetAutoresPorNombreContiene(string texto);
        Task<IEnumerable<AutorInsertDTO>> GetAutoresPaginados(int desde, int hasta);
        Task<AutorInsertDTO> Add(AutorInsertDTO autorInsertDTO);
       
    }
}