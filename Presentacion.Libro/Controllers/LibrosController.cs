using Aplication.Dtos;
using Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EntidadLibro = Domian.Entidades.Libro;

namespace Presentacion.Controllers
{

    public class LibrosController : Controller
    {
        private readonly ILibroService _service;

        public LibrosController(ILibroService service)
        {
            _service = service;
        }

        //  LISTAR (READ)
        public async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario")))
                return RedirectToAction("Login", "Acceso");

          
            var libros = await _service.GetAllLibrosAsync();
            return View(libros);
        }

        //  CREAR (CREATE)
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Usuario"))) return RedirectToAction("Login", "Acceso");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EntidadLibro libro)
        {
            
            ModelState.Remove("Id");
            ModelState.Remove("Eliminado");

          
            if (libro.Titulo != null && libro.Titulo.Length > 50)
            {
                ModelState.AddModelError("Titulo", "El título es muy largo");
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

        //  EDITAR (UPDATE)
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
           
            ModelState.Remove("Eliminado");

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
        

        // ELIMINAR (DELETE - Lógico)
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
        
            await _service.DeleteLibroAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}


