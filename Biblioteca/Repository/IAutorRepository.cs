using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface IAutorRepository : IBaseRepository<Autor>
    {
        Task<IEnumerable<Libro>> GetAutoresConDetalles();
        Task<AutorLibroDTO?> GetAutorLibrosSelect(int id);
        Task<bool> ExisteAutor(int autorId);
        Task<IEnumerable<Autor>> GetAutoresOrdenadosPorNombre(bool ascendente);
        Task<IEnumerable<Autor>> GetAutoresPorNombreContiene(string texto);
        Task<IEnumerable<Autor>> GetAutoresPaginados(int desde, int hasta);
    }
}

