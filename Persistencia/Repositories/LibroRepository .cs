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

        // 1. MODIFICAR GETALL: Solo traer los que NO están eliminados
        public async Task<IEnumerable<Libro>> GetAllAsync()
        {
            return await _context.Libros
                .Where(l => l.Eliminado == false) // <--- FILTRO LÓGICO
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

        // 2. MODIFICAR DELETE: No borrar, solo marcar como Eliminado
        public async Task DeleteAsync(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                // EN LUGAR DE _context.Libros.Remove(libro);

                libro.Eliminado = true; // <--- BORRADO LÓGICO
                _context.Libros.Update(libro); // Lo actualizamos como "modificado"

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Libro>> SearchAsync(string searchTerm)
        {
            return await _context.Libros
                .Where(l => !l.Eliminado && // <--- Agregar filtro aquí también
                           (l.Titulo.Contains(searchTerm) ||
                            l.Autor.Contains(searchTerm) ||
                            l.Isbn.Contains(searchTerm)))
                .ToListAsync();
        }
    }
}
