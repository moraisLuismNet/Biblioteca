using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface ILibroService : ICommonServiceBase<LibroDTO, LibroInsertDTO, LibroUpdateDTO>
    {
        Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios();
        Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado();
        Task<IEnumerable<LibroDTO>> GetLibrosPaginados(int desde, int hasta);
        Task<IEnumerable<LibroDTO>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax);
        Task<IEnumerable<LibroDTO>> GetLibrosOrdenadosPorTitulo(bool ascendente);
        Task<IEnumerable<LibroDTO>> GetLibrosPorTituloContiene(string texto);
        Task<Libro> GetLibroPorId(int id);
        Task EliminarLibro(LibroDTO libro);
        Task<LibroDTO> Add(LibroInsertDTO libroInsertDTO);
    }
}