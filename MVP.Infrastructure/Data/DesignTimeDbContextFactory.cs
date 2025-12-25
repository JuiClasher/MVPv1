using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
// Agregar el using para el namespace donde está ApplicationDbContext
using MVP.Infrastructure.Data;

namespace MVP.Infrastructure.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Preferir variable de entorno en CI; fallback a placeholder.
            var conn = Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DEFAULTCONNECTION")
                       ?? "Host=localhost;Port=5432;Database=MVP;Username=postgres;Password=1234";

            builder.UseNpgsql(conn, npgsql => npgsql.EnableRetryOnFailure());
            return new ApplicationDbContext(builder.Options);
        }
    }
}