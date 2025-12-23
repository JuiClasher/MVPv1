using System;

namespace MVP.Core.Entities
{
    public class StockPorSucursal
    {
        public Guid StockSucursalID { get; set; }
        public Guid SucursalID { get; set; }
        public Sucursal? Sucursal { get; set; }

        public Guid ProductoID { get; set; }
        public Producto? Producto { get; set; }

        public int CantidadActual { get; set; }
        public int? StockMinimoSucursal { get; set; }
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public StockPorSucursal()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);
        }
    }
}