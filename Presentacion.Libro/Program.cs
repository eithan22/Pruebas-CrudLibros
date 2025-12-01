using Aplication.Interfaces;
using Aplication.Services;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using Persistencia.Repositories;

namespace Presentacion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Conexión a Base de Datos 
            builder.Services.AddDbContext<AppBDContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Inyección de tus Servicios 
            builder.Services.AddScoped<ILibroRepository, LibroRepository>();
            builder.Services.AddScoped<ILibroService, LibroService>();

          
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

          
            app.UseSession();
            app.UseAuthorization();

         
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Acceso}/{action=Login}/{id?}");

            app.Run();

        }
    }
}
