using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IEditorialService : ICommonServiceBase<EditorialDTO, EditorialInsertDTO, EditorialUpdateDTO>
    {
        Task<EditorialLibroDTO> GetEditorialesLibrosEager(int id);
        Task<IEnumerable<Editorial>> GetEditorialesOrdenadasPorNombre(bool ascendente);
        Task<IEnumerable<Editorial>> GetEditorialesPorNombreContiene(string texto);
        Task<IEnumerable<Editorial>> GetEditorialesPaginadas(int desde, int hasta);
    }
}