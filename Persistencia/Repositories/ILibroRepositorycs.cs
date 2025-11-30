using Domian.Entidades;


namespace Persistencia.Repositories
{
    public interface ILibroRepository
    {
        Task<IEnumerable<Libro>> GetAllAsync();
        Task<Libro?> GetByIdAsync(int id);
        Task<Libro> AddAsync(Libro libro);
        Task UpdateAsync(Libro libro);
        Task DeleteAsync(int id);
        Task<IEnumerable<Libro>> SearchAsync(string searchTerm);
    }

}
