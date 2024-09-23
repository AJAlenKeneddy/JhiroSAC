namespace JhiroServer.Models
{
    public class GuardarCarritoRequest
    {
        public int UsuarioId { get; set; }
        public List<CarritoDTO> CarritoItems { get; set; }
    }

}
