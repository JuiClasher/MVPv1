using System;

namespace MVP.Core.Entities
{
    public class Categoria
    {
        public Guid CategoriaID { get; set; }
        public Guid EmpresaID { get; set; }
        public Empresa? Empresa { get; set; }

        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public Categoria()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);
        }
    }
}