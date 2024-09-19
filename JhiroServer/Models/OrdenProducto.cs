using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class OrdenProducto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public int OrdenId { get; set; }

        public virtual Orden Orden { get; set; } = null!;
    }
}
