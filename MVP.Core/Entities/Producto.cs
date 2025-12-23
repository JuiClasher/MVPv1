using System;
using System.Collections.Generic;

namespace MVP.Core.Entities
{
    public class Producto
    {
        public Guid ProductoID { get; set; }
        public Guid EmpresaID { get; set; }
        public Empresa? Empresa { get; set; }

        public Guid CategoriaID { get; set; }
        public Categoria? Categoria { get; set; }

        public Guid MarcaID { get; set; }
        public Marca? Marca { get; set; }

        public Guid UnidadDeMedidaID { get; set; }
        public UnidadDeMedida? UnidadDeMedida { get; set; }

        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string CodigoSKU { get; set; } = null!;
        public string? CodigoBarras { get; set; }
        public decimal? PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal? PrecioMayorista { get; set; }
        public int? StockMinimo { get; set; }
        public bool Activo { get; set; } = true;
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public ICollection<ProductoProveedor> Proveedores { get; set; }
        public ICollection<ImagenDelProducto> Imagenes { get; set; }
        public ICollection<StockPorSucursal> StockPorSucursales { get; set; }

        public Producto()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);

            Proveedores = new HashSet<ProductoProveedor>();
            Imagenes = new HashSet<ImagenDelProducto>();
            StockPorSucursales = new HashSet<StockPorSucursal>();
        }
    }
}