using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface IAutorRepository : IBaseRepository<Autor>
    {
        Task<IEnumerable<Libro>> GetAutoresConDetalles();
        Task<AutorLibroDTO?> GetAutorLibrosSelect(int id);
        Task<bool> ExisteAutor(int autorId);

        //Task<EditorialLibroDTO?> GetEditorialesLibrosEager(int id);
        
        
        
        
        //Task<bool> ExisteEditorial(int editorialId);
    }
}

