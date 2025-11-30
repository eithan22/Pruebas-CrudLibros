using Microsoft.AspNetCore.Mvc;
using Domian.Entidades;          
using Aplication.Dtos;          
using Aplication.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase
    {
        private readonly ILibroService _libroService;

        public LibrosController(ILibroService libroService)
        {
            _libroService = libroService;
        }

        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibros()
        {
            var libros = await _libroService.GetAllLibrosAsync();
            return Ok(libros);
        }

        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id)
        {
            var libro = await _libroService.GetLibroByIdAsync(id);

            if (libro == null)
            {
                return NotFound(new { message = "Libro no encontrado" });
            }

            return Ok(libro);
        }

        // POST: api/Libros
        [HttpPost]
        public async Task<ActionResult<Libro>> CreateLibro(LibroDto libroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var libro = await _libroService.CreateLibroAsync(libroDto);
            return CreatedAtAction(nameof(GetLibro), new { id = libro.Id }, libro);
        }

        // PUT: api/Libros/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLibro(int id, LibroDto libroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _libroService.UpdateLibroAsync(id, libroDto);

            if (!result) return NotFound(new { message = "Libro no encontrado" });

            return NoContent();
        }

        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id)
        {
            var result = await _libroService.DeleteLibroAsync(id);

            if (!result) return NotFound(new { message = "Libro no encontrado" });

            return NoContent();
        }

        // GET: api/Libros/search?term=...
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Libro>>> SearchLibros([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest(new { message = "El término de búsqueda es requerido" });

            var libros = await _libroService.SearchLibrosAsync(term);
            return Ok(libros);
        }
    }

}