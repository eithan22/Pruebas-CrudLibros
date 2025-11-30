using Domian.Entidades;
using Microsoft.EntityFrameworkCore;


namespace Persistencia.Data
{
    public class AppBDContext : DbContext
    {
        public AppBDContext(DbContextOptions<AppBDContext> options) : base(options)
        {
        }

        public DbSet<Libro> Libros { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>(entity => 
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Autor).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Isbn).IsRequired().HasMaxLength(13);
                entity.HasIndex(e => e.Isbn).IsUnique();
            });

            // Datos iniciales
            modelBuilder.Entity<Libro>().HasData( 
                new Libro
                {
                    Id = 1,
                    Titulo = "Cien Años de Soledad",
                    Autor = "Gabriel García Márquez",
                    Isbn = "9780060883287"
                },
                new Libro
                {
                    Id = 2,
                    Titulo = "Don Quijote de la Mancha",
                    Autor = "Miguel de Cervantes",
                    Isbn = "9788420412146"
                }
            );
        }
    }
}
