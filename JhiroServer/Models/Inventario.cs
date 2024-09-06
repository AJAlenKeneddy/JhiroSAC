using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class Inventario
    {
        public int InventarioId { get; set; }
        public int? ProductoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaActualizacion { get; set; }

        public virtual Producto? Producto { get; set; }
    }
}
