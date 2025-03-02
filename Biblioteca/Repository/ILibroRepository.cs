using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface ILibroRepository : IBaseRepository<Libro>
    {
        Task<bool> LibroExists(int id);
        Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios();
        Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado();
        Task<IEnumerable<LibroDTO>> GetLibrosPaginados(int desde, int hasta);
        Task<IEnumerable<LibroDTO>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax);
        Task<IEnumerable<LibroDTO>> GetLibrosOrdenadosPorTitulo(bool ascendente);
        Task<IEnumerable<LibroDTO>> GetLibrosPorTituloContiene(string texto);
        Task<Libro> GetLibroPorId(int id);
        Task<bool> ExisteAutor(int autorId);
        Task<bool> ExisteEditorial(int editorialId);

        Task<IEnumerable<LibroDTO>> Get();

        Task<LibroDTO> GetById(int id);

        Task Add(LibroInsertDTO libroInsertDTO);

        Task Update(LibroUpdateDTO libroUpdateDTO);
        Task DeleteLibro(LibroDTO libro);
    }
}
