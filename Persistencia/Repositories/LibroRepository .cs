using Domian.Entidades;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;


namespace Persistencia.Repositories
{
    public class LibroRepository : ILibroRepository
    {
        private readonly AppBDContext _context;

        public LibroRepository(AppBDContext context)
        {
            _context = context;
        }

       
        public async Task<IEnumerable<Libro>> GetAllAsync()
        {
            return await _context.Libros
                .Where(l => l.Eliminado == false) 
                .ToListAsync();
        }

        public async Task<Libro?> GetByIdAsync(int id)
        {
            return await _context.Libros
                .FirstOrDefaultAsync(l => l.Id == id && l.Eliminado == false);
        }

        public async Task<Libro> AddAsync(Libro libro)
        {
            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();
            return libro;
        }

        public async Task UpdateAsync(Libro libro)
        {
            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();
        }

      
        public async Task DeleteAsync(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
             

                libro.Eliminado = true; 
                _context.Libros.Update(libro); 

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Libro>> SearchAsync(string searchTerm)
        {
            return await _context.Libros
                .Where(l => !l.Eliminado && 
                           (l.Titulo.Contains(searchTerm) ||
                            l.Autor.Contains(searchTerm) ||
                            l.Isbn.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
