using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class OrdenProducto
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }
        public int UsuarioId { get; set; }

        public virtual Carrito Carrito { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
    }
}
