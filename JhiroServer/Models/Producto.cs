using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Inventarios = new HashSet<Inventario>();
        }

        public int ProductoId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string? ImagenUrl { get; set; }
        public int? CategoriaId { get; set; }
        public bool Eliminado { get; set; }

        public virtual Categoria? Categoria { get; set; }
        public virtual ICollection<Inventario> Inventarios { get; set; }
    }
}
