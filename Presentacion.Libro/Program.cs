using Aplication.Interfaces;
using Aplication.Services;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using Persistencia.Repositories;

namespace Presentacion.Libro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Conexión a Base de Datos (Copia la conexión de la API a tu appsettings.json de Presentacion)
            builder.Services.AddDbContext<AppBDContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Inyección de tus Servicios (Conectamos el Frontend con tu Lógica)
            builder.Services.AddScoped<ILibroRepository, LibroRepository>();
            builder.Services.AddScoped<ILibroService, LibroService>();

            // 3. Configurar Sesión (Obligatorio para el Login)
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configuración de errores y archivos estáticos
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            // 4. ORDEN IMPORTANTE: Primero Sesión, luego Autorización
            app.UseSession();
            app.UseAuthorization();

            // 5. RUTA DE INICIO (Aquí le decimos que arranque en Acceso/Login)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acceso}/{action=Login}/{id?}");

            app.Run();

        }
    }
}
