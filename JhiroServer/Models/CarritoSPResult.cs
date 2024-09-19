using System.ComponentModel.DataAnnotations;

namespace JhiroServer.Models
{
    public class CarritoSPResult
    {
        [Key]
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal PrecioProducto { get; set; }
        public int Cantidad { get; set; }
        public string ImagenUrl { get; set; }
    }

}
