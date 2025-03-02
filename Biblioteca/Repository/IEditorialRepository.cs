using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface IEditorialRepository : IBaseRepository<Editorial>
    {
        Task<EditorialLibroDTO?> GetEditorialLibrosSelect(int id);
        Task<IEnumerable<EditorialInsertDTO>> GetEditorialesOrdenadasPorNombre(bool ascendente);
        Task<IEnumerable<EditorialInsertDTO>> GetEditorialesPorNombreContiene(string texto);
        Task<IEnumerable<EditorialInsertDTO>> GetEditorialesPaginadas(int desde, int hasta);
        Task<IEnumerable<EditorialDTO>> Get();
        Task Add(EditorialInsertDTO editorialInsertDTO);
        Task Update(EditorialUpdateDTO editorialUpdateDTO);
        Task<Editorial> GetById(int id);
        void Delete(Editorial editorial);

    }
}

