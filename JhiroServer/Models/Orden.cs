using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class Orden
    {
        public Orden()
        {
            OrdenProductos = new HashSet<OrdenProducto>();
        }

        public int Id { get; set; }
        public string CorreoElectronico { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public decimal Total { get; set; }

        public virtual ICollection<OrdenProducto> OrdenProductos { get; set; }
    }
}
