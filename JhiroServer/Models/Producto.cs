using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JhiroServer.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Inventarios = new HashSet<Inventario>();
        }

        public int ProductoId { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "Descripcion requerida.")]
        public string? Descripcion { get; set; }
        [Required(ErrorMessage = "Precio es requerido.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "Stock es requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "El stock debe ser mayor que 0.")]
        public int Stock { get; set; }
        [Required(ErrorMessage = "URL es requerida.")]
        public string? ImagenUrl { get; set; }
        [Required(ErrorMessage = "Categoria es requerida.")]
        public int? CategoriaId { get; set; }
        public bool Eliminado { get; set; }

        public virtual Categoria? Categoria { get; set; }
        public virtual ICollection<Inventario> Inventarios { get; set; }
    }
}
