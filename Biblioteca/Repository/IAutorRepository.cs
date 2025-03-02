using Biblioteca.DTOs;
using Biblioteca.Models;

namespace Biblioteca.Repository
{
    public interface IAutorRepository : IBaseRepository<Autor>
    {
        
        Task<IEnumerable<AutorLibroDTO>> GetAutoresConDetalles();
        Task<AutorLibroDTO?> GetAutorLibrosSelect(int id);
        Task<IEnumerable<AutorInsertDTO>> GetAutoresOrdenadosPorNombre(bool ascendente);
        Task<IEnumerable<AutorInsertDTO>> GetAutoresPorNombreContiene(string texto);
        Task<IEnumerable<AutorInsertDTO>> GetAutoresPaginados(int desde, int hasta);
        Task<IEnumerable<AutorDTO>> Get();
        Task Add(AutorInsertDTO autorInsertDTO);
        Task Update(AutorUpdateDTO autorUpdateDTO);

        Task<Autor> GetById(int id);

        void Delete(Autor autor);
    }
}

