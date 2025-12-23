using System;
using System.Collections.Generic;

namespace MVP.Core.Entities
{
    public class Sucursal
    {
        public Guid SucursalID { get; set; }
        public Guid EmpresaID { get; set; }
        public Empresa? Empresa { get; set; }

        public string Nombre { get; set; } = null!;
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Provincia { get; set; }
        public string? Pais { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; } = true;
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public ICollection<StockPorSucursal> StockPorSucursales { get; set; }

        public Sucursal()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);
            StockPorSucursales = new HashSet<StockPorSucursal>();
        }
    }
}