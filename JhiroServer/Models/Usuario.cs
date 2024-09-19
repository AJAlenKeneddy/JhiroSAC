using System;
using System.Collections.Generic;

namespace JhiroServer.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Carritos = new HashSet<Carrito>();
        }

        public int UsuarioId { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Correo { get; set; } = null!;

        public virtual ICollection<Carrito> Carritos { get; set; }
    }
}
