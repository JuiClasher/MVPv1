using System;

namespace MVP.Core.Entities
{
    public class ImagenDelProducto
    {
        public Guid ImagenDelProductoID { get; set; }
        public Guid ProductoID { get; set; }
        public Producto? Producto { get; set; }

        public string URLImagen { get; set; } = null!;
        public bool EsPrincipal { get; set; } = false;
        public int? OrdenVisual { get; set; }
        public DateTimeOffset FechaCreacion { get; set; }
        public DateTimeOffset FechaModificacion { get; set; }

        public ImagenDelProducto()
        {
            var argOffset = TimeSpan.FromHours(-3);
            FechaCreacion = FechaModificacion = DateTimeOffset.Now.ToOffset(argOffset);
        }
    }
}