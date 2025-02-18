using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Repository
{
    public class EditorialRepository : IRepository<Editorial>
    {
        private BibliotecaContext _context;

        public EditorialRepository(BibliotecaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Editorial>> Get() =>
            await _context.Editoriales.ToListAsync();

        public async Task<Editorial> GetById(int id) =>
            await _context.Editoriales.FindAsync(id);

        public async Task Add(Editorial editorial) =>
            await _context.Editoriales.AddAsync(editorial);

        public void Update(Editorial editorial)
        {
            _context.Editoriales.Attach(editorial);
            _context.Entry(editorial).State = EntityState.Modified;
        }

        public void Delete(Editorial editorial) =>
           _context.Editoriales.Remove(editorial);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<Editorial> Search(Func<Editorial, bool> filter) =>
        _context.Editoriales.Where(filter).ToList();
    }
}
