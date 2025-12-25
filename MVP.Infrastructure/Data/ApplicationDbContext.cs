using Microsoft.EntityFrameworkCore;

namespace MVP.Infrastructure.Data
{
    // Placeholder mínimo para compilar. Será reemplazado por el DbContext generado
    // por el scaffold o por tu implementación definitiva después de importar la BD.
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Los DbSet<> se generarán después al importar la BD.
    }
}