using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface IEditorialService : ICommonServiceBase<EditorialDTO, EditorialInsertDTO, EditorialUpdateDTO>
    {
        Task<EditorialLibroDTO> GetEditorialesLibrosEager(int id);
    }
}