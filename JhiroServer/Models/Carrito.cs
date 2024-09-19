using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JhiroServer.Models
{
    public partial class Carrito
    {
        [Key]
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }

        public virtual Producto Producto { get; set; }
        public virtual Usuario Usuario { get; set; }
    }

}
