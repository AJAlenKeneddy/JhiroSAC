namespace JhiroServer.Models
{
    public class ProductoPedidoDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; } 
    }

}
