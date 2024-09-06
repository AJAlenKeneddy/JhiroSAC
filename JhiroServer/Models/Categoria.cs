using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class Categoria
    {
        public Categoria()
        {
            Productos = new HashSet<Producto>();
        }

        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Eliminado { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
