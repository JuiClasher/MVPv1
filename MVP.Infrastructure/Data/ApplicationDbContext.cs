using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVP.Core.Entities;

namespace MVP.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private static readonly TimeSpan ArgOffset = TimeSpan.FromHours(-3);

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Empresa> Empresas { get; set; } = null!;
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Marca> Marcas { get; set; } = null!;
        public DbSet<UnidadDeMedida> UnidadesDeMedida { get; set; } = null!;
        public DbSet<Proveedor> Proveedores { get; set; } = null!;
        public DbSet<Producto> Productos { get; set; } = null!;
        public DbSet<ProductoProveedor> ProductoProveedores { get; set; } = null!;
        public DbSet<ImagenDelProducto> ImagenesDelProducto { get; set; } = null!;
        public DbSet<Sucursal> Sucursales { get; set; } = null!;
        public DbSet<StockPorSucursal> StockPorSucursal { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Mapeo a tablas existentes en minúscula — no aplica cambios a la BD, solo modelado cliente.
            modelBuilder.Entity<Empresa>().ToTable("empresas");
            modelBuilder.Entity<Categoria>().ToTable("categorias");
            modelBuilder.Entity<Marca>().ToTable("marcas");
            modelBuilder.Entity<UnidadDeMedida>().ToTable("unidadesdemedida");
            modelBuilder.Entity<Proveedor>().ToTable("proveedores");
            modelBuilder.Entity<Producto>().ToTable("productos");
            modelBuilder.Entity<ProductoProveedor>().ToTable("productoproveedor");
            modelBuilder.Entity<ImagenDelProducto>().ToTable("imagenesdelproducto");
            modelBuilder.Entity<Sucursal>().ToTable("sucursales");
            modelBuilder.Entity<StockPorSucursal>().ToTable("stockporsucursal");

            // Opcional: configurar claves por convención y defaults que existen en la BD (no crean DDL hasta migrar)
            modelBuilder.Entity<Empresa>().HasKey(e => e.EmpresaID);
            modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaID);
            modelBuilder.Entity<Marca>().HasKey(m => m.MarcaID);
            modelBuilder.Entity<UnidadDeMedida>().HasKey(u => u.UnidadDeMedidaID);
            modelBuilder.Entity<Proveedor>().HasKey(p => p.ProveedorID);
            modelBuilder.Entity<Producto>().HasKey(p => p.ProductoID);
            modelBuilder.Entity<ProductoProveedor>().HasKey(pp => pp.ProductoProveedorID);
            modelBuilder.Entity<ImagenDelProducto>().HasKey(i => i.ImagenDelProductoID);
            modelBuilder.Entity<Sucursal>().HasKey(s => s.SucursalID);
            modelBuilder.Entity<StockPorSucursal>().HasKey(s => s.StockSucursalID);

            // Marcas temporales de columna para tipos y timestamps (coincidir con BD)
            modelBuilder.Entity<Empresa>().Property(e => e.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Empresa>().Property(e => e.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Categoria>().Property(c => c.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Categoria>().Property(c => c.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Marca>().Property(m => m.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Marca>().Property(m => m.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<UnidadDeMedida>().Property(u => u.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<UnidadDeMedida>().Property(u => u.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Proveedor>().Property(p => p.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Proveedor>().Property(p => p.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Producto>().Property(p => p.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Producto>().Property(p => p.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<ProductoProveedor>().Property(pp => pp.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<ProductoProveedor>().Property(pp => pp.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<ImagenDelProducto>().Property(i => i.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<ImagenDelProducto>().Property(i => i.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Sucursal>().Property(s => s.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<Sucursal>().Property(s => s.FechaModificacion).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<StockPorSucursal>().Property(s => s.FechaCreacion).HasColumnType("timestamp with time zone");
            modelBuilder.Entity<StockPorSucursal>().Property(s => s.FechaModificacion).HasColumnType("timestamp with time zone");

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var now = DateTimeOffset.Now.ToOffset(ArgOffset);
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Properties.Any(p => p.Metadata.Name == "FechaModificacion"))
                {
                    entry.Property("FechaModificacion").CurrentValue = now;
                }

                if (entry.State == EntityState.Added && entry.Properties.Any(p => p.Metadata.Name == "FechaCreacion"))
                {
                    var prop = entry.Property("FechaCreacion");
                    if (prop.CurrentValue == null || prop.CurrentValue.Equals(default(DateTimeOffset)))
                    {
                        prop.CurrentValue = now;
                    }
                }
            }
        }
    }
}