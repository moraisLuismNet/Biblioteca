using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface ILibroService : ICommonServiceBase<LibroDTO, LibroInsertDTO, LibroUpdateDTO>
    {
        Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios();
        Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado();
        Task<IEnumerable<Libro>> GetLibrosPaginados(int desde, int hasta);
        Task<IEnumerable<Libro>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax);
        Task<IEnumerable<Libro>> GetLibrosOrdenadosPorTitulo(bool ascendente);
        Task<IEnumerable<Libro>> GetLibrosPorTituloContiene(string texto);
        Task<Libro> GetLibroPorId(int id);
        Task EliminarLibro(Libro libro);
    }
}