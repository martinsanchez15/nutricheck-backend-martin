using Microsoft.EntityFrameworkCore;
using NutriCheck.Models;

namespace NutriCheck.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Nutricionista> Nutricionistas { get; set; }
        public DbSet<Comida> Comidas { get; set; }
        public DbSet<PlatoComida> PlatosComida { get; set; }
        public DbSet<Dieta> Dietas { get; set; }
        public DbSet<Preferencia> Preferencias { get; set; }
        
        







    }
}
