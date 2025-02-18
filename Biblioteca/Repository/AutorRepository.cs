using Biblioteca.Models;
using Biblioteca.Repository;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repository
{
    public class AutorRepository : IRepository<Autor>
    {
        private BibliotecaContext _context;

        public AutorRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Autor>> Get() =>
            await _context.Autores.ToListAsync();

        public async Task<Autor> GetById(int id) =>
            await _context.Autores.FindAsync(id);

        public async Task Add(Autor autor) =>
            await _context.Autores.AddAsync(autor);

        public void Update(Autor autor)
        {
            _context.Autores.Attach(autor);
            _context.Entry(autor).State = EntityState.Modified;
        }

        public void Delete(Autor autor) =>
           _context.Autores.Remove(autor);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<Autor> Search(Func<Autor, bool> filter) =>
        _context.Autores.Where(filter).ToList();
    }
}

