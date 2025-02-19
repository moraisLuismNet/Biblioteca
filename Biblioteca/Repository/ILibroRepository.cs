using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface ILibroRepository : IBaseRepository<Libro>
    {
        Task<bool> LibroExists(int id);
        Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios();
        Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado();
        Task<IEnumerable<Libro>> GetLibrosPaginados(int desde, int hasta);
        Task<IEnumerable<Libro>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax);
        Task<IEnumerable<Libro>> GetLibrosOrdenadosPorTitulo(bool ascendente);
        Task<IEnumerable<Libro>> GetLibrosPorTituloContiene(string texto);
        Task<Libro> GetLibroPorId(int id);
        Task DeleteLibro(Libro libro);
        Task<bool> ExisteAutor(int autorId);
        Task<bool> ExisteEditorial(int editorialId);
    }
}
