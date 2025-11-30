using Microsoft.AspNetCore.Mvc;

namespace Presentacion.Libro.Controllers
{
    public class AccesoController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Validar(string usuario, string password)
        {
            // Login simulado para la tarea
            if (usuario == "admin" && password == "123")
            {
                HttpContext.Session.SetString("Usuario", usuario);
                return RedirectToAction("Index", "Libros");
            }
            else
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }
        }
    }
}
