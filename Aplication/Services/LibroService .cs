using Aplication.Dtos;
using Aplication.Interfaces;
using Domian.Entidades;
using Persistencia.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _repository;

        public LibroService(ILibroRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Libro>> GetAllLibrosAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Libro?> GetLibroByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Libro> CreateLibroAsync(LibroDto libroDto)
        {
            var libro = new Libro
            {
                Titulo = libroDto.Titulo,
                Autor = libroDto.Autor,
                Isbn = libroDto.Isbn
            };

            return await _repository.AddAsync(libro);
        }

        public async Task<bool> UpdateLibroAsync(int id, LibroDto libroDto)
        {
            var libro = await _repository.GetByIdAsync(id);
            if (libro == null) return false;

            libro.Titulo = libroDto.Titulo;
            libro.Autor = libroDto.Autor;
            libro.Isbn = libroDto.Isbn;

            await _repository.UpdateAsync(libro);
            return true;
        }

        public async Task<bool> DeleteLibroAsync(int id)
        {
            var libro = await _repository.GetByIdAsync(id);
            if (libro == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<IEnumerable<Libro>> SearchLibrosAsync(string searchTerm)
        {
            return await _repository.SearchAsync(searchTerm);
        }
    }
}
