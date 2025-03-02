using Biblioteca.DTOs;

namespace Biblioteca.Services
{
    public interface IEditorialService : ICommonServiceBase<EditorialDTO, EditorialInsertDTO, EditorialUpdateDTO>
    {
        Task<EditorialLibroDTO> GetEditorialLibrosSelect(int id);
        Task<IEnumerable<EditorialInsertDTO>> GetEditorialesOrdenadasPorNombre(bool ascendente);
        Task<IEnumerable<EditorialInsertDTO>> GetEditorialesPorNombreContiene(string texto);
        Task<IEnumerable<EditorialInsertDTO>> GetEditorialesPaginadas(int desde, int hasta);
        Task<EditorialInsertDTO> Add(EditorialInsertDTO editorialInsertDTO);
    }
}