using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// Le indicamos que desde el DbContext pueda invocar nuestra base. El connectionstring estará en el appSettings.json en la raiz de la API
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(new Villa
            {
                Id = 1,
                Nombre = "Villa Real",
                Detalle = "Detalle de la villa...",
                ImagenUrl = "",
                Ocupantes = 5,
                Metros2 = 50,
                Tarifa = 200,
                Amenidad = "",
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            }, new Villa
            {
                Id = 2,
                Nombre = "Premium Vista a la Piscina",
                Detalle = "Detalle de la villa...",
                ImagenUrl = "",
                Ocupantes = 4,
                Metros2 = 50,
                Tarifa = 150,
                Amenidad = "",
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now
            });
           // base.OnModelCreating(modelBuilder);
        }
    }
}
