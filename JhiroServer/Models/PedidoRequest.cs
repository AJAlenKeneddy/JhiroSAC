using static JhiroServer.Controllers.AccesoController;

namespace JhiroServer.Models
{
    public class PedidoRequest
    {
        public string Correo { get; set; }
        public List<ProductoPedidoDTO> Productos { get; set; }
    }
}
