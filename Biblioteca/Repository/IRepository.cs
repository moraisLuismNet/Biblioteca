using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> Get();
        Task<TEntity> GetById(int id);
        Task Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task Save();
        IEnumerable<TEntity> Search(Func<TEntity, bool> filter);
        Task<bool> LibroExists(int id);
        Task<IEnumerable<Libro>> GetAutoresConDetalles();
        Task<AutorLibroDTO?> GetAutorLibrosSelect(int id);
        Task<EditorialLibroDTO?> GetEditorialesLibrosEager(int id);
        Task<IEnumerable<LibroVentaDTO>> GetLibrosYPrecios();
        Task<IEnumerable<LibroGroupDTO>> GetLibrosGroupedByDescatalogado();
        Task<IEnumerable<Libro>> GetLibrosPaginados(int desde, int hasta);
        Task<IEnumerable<Libro>> GetLibrosPorPrecio(decimal precioMin, decimal precioMax);
        Task<IEnumerable<Libro>> GetLibrosOrdenadosPorTitulo(bool ascendente);
        Task<IEnumerable<Libro>> GetLibrosPorTituloContiene(string texto);
        Task<bool> ExisteAutor(int autorId);
        Task<Libro> GetLibroPorId(int id);
        Task DeleteLibro(Libro libro);
        Task<bool> ExisteEditorial(int editorialId);
    }
}
