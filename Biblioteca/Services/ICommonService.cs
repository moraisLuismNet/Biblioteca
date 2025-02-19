using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Services
{
    public interface ICommonService<T, TI, TU>
    {
        public List<string> Errors { get; }
        Task<IEnumerable<T>> Get();
        Task<T> GetById(int id);
        Task<T> Add(TI tInsertDTO);
        Task<T> Update(int id, TU tUpdateDTO);
        Task<T> Delete(int id);
        bool Validate(TI dto);
        bool Validate(TU dto);
        Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles();
        Task<AutorLibroDTO> GetAutorLibrosSelect(int id);
        Task<EditorialLibroDTO> GetEditorialesLibrosEager(int id);
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
