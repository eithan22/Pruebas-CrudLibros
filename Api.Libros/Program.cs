
using Aplication.Interfaces;
using Aplication.Services;
using Microsoft.EntityFrameworkCore;
using Persistencia.Data;
using Persistencia.Repositories;

namespace Api.Libros
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            // 2.  (Conexión a SQL Server)
            builder.Services.AddDbContext<AppBDContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 3. INYECCIÓN DE DEPENDENCIAS 
            
            builder.Services.AddScoped<ILibroRepository, LibroRepository>();
            builder.Services.AddScoped<ILibroService, LibroService>();



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
