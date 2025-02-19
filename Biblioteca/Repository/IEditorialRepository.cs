using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface IEditorialRepository : IBaseRepository<Editorial>
    {
        Task<EditorialLibroDTO?> GetEditorialesLibrosEager(int id);
        Task<bool> ExisteEditorial(int editorialId);
    }
}

