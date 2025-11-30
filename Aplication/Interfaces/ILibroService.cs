using Aplication.Dtos;
using Domian.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface ILibroService
    {
        Task<IEnumerable<Libro>> GetAllLibrosAsync();
        Task<Libro?> GetLibroByIdAsync(int id);
        Task<Libro> CreateLibroAsync(LibroDto libroDto);
        Task<bool> UpdateLibroAsync(int id, LibroDto libroDto);
        Task<bool> DeleteLibroAsync(int id);
        Task<IEnumerable<Libro>> SearchLibrosAsync(string searchTerm);
    }
}
