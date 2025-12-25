using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MVP.Infrastructure.Data;

namespace MVP.UI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var conn = configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("CONNECTIONSTRINGS__DEFAULTCONNECTION")
                       ?? throw new InvalidOperationException("No connection string configured.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(conn, o => o.EnableRetryOnFailure()));

            return services;
        }
    }
}