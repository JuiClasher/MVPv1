using System;
using System.Collections.Generic;

namespace MVP.Core.Entities
{
    public class Empresa
    {
        public Guid EmpresaID { get; set; }
        public string RazonSocial { get; set; } = null!;
        public string? CUIT { get; set; }
        public string? DireccionFiscal { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public bool Activo { get; set; } = true;
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public ICollection<Categoria> Categorias { get; set; }
        public ICollection<Marca> Marcas { get; set; }
        public ICollection<UnidadDeMedida> UnidadesDeMedida { get; set; }
        public ICollection<Proveedor> Proveedores { get; set; }
        public ICollection<Producto> Productos { get; set; }
        public ICollection<Sucursal> Sucursales { get; set; }

        public Empresa()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);

            Categorias = new HashSet<Categoria>();
            Marcas = new HashSet<Marca>();
            UnidadesDeMedida = new HashSet<UnidadDeMedida>();
            Proveedores = new HashSet<Proveedor>();
            Productos = new HashSet<Producto>();
            Sucursales = new HashSet<Sucursal>();
        }
    }
}