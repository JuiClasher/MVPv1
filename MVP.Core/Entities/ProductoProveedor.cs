using System;

namespace MVP.Core.Entities
{
    public class ProductoProveedor
    {
        public Guid ProductoProveedorID { get; set; }

        public Guid ProductoID { get; set; }
        public Producto? Producto { get; set; }

        public Guid ProveedorID { get; set; }
        public Proveedor? Proveedor { get; set; }

        public decimal? PrecioProveedor { get; set; }
        public bool Activo { get; set; } = true;
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public ProductoProveedor()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);
        }
    }
}