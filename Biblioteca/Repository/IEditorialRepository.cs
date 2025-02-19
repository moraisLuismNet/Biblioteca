using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface IEditorialRepository : IBaseRepository<Editorial>
    {
        Task<EditorialLibroDTO?> GetEditorialesLibrosEager(int id);
        Task<bool> ExisteEditorial(int editorialId);
        Task<IEnumerable<Editorial>> GetEditorialesOrdenadasPorNombre(bool ascendente);
        Task<IEnumerable<Editorial>> GetEditorialesPorNombreContiene(string texto);
        Task<IEnumerable<Editorial>> GetEditorialesPaginadas(int desde, int hasta);
    }
}

