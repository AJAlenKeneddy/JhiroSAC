using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class Carrito
    {
        public Carrito()
        {
            OrdenProductos = new HashSet<OrdenProducto>();
        }

        public int CarritoId { get; set; }
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        public virtual Producto Producto { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual ICollection<OrdenProducto> OrdenProductos { get; set; }
    }
}
