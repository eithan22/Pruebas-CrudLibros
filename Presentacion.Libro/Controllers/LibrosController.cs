using Aplication.Dtos;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntidadLibro = Domian.Entidades.Libro;

namespace Presentacion.Libro.Controllers
{

    public class LibrosController : Controller
    {
        private readonly ILibroService _service;

        public LibrosController(ILibroService service)
        {
            _service = service;
        }

        // 1. LISTAR (READ)
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
                return RedirectToAction("Login", "Acceso");

            // Tu repositorio modificado ya filtra los eliminados automáticamente
            var libros = await _service.GetAllLibrosAsync();
            return View(libros);
        }

        // 2. CREAR (CREATE)
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction("Login", "Acceso");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EntidadLibro libro)
        {
            // Validación manual para Prueba de Límites (Selenium)
            if (libro.Titulo != null && libro.Titulo.Length > 50)
            {
                ModelState.AddModelError("Titulo", "El título excede el límite permitido");
                return View(libro);
            }

            if (ModelState.IsValid)
            {
                var dto = new LibroDto
                {
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    Isbn = libro.Isbn
                };
                await _service.CreateLibroAsync(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // 3. EDITAR (UPDATE)
        public async Task<IActionResult> Edit(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction("Login", "Acceso");

            var libro = await _service.GetLibroByIdAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EntidadLibro libro)
        {
            if (ModelState.IsValid)
            {
                var dto = new LibroDto
                {
                    Titulo = libro.Titulo,
                    Autor = libro.Autor,
                    Isbn = libro.Isbn
                };
                await _service.UpdateLibroAsync(libro.Id, dto);
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        // 4. ELIMINAR (DELETE - Lógico)
        public async Task<IActionResult> Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction("Login", "Acceso");

            var libro = await _service.GetLibroByIdAsync(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Esto llama al repositorio que modificamos para hacer Soft Delete
            await _service.DeleteLibroAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}


