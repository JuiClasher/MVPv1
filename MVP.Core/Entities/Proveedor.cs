using System;

namespace MVP.Core.Entities
{
    public class Proveedor
    {
        public Guid ProveedorID { get; set; }
        public Guid EmpresaID { get; set; }
        public Empresa? Empresa { get; set; }

        public string Nombre { get; set; } = null!;
        public string? CUIT { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public Proveedor()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);
        }
    }
}