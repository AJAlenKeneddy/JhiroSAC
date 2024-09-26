using System.Net.NetworkInformation;
using static JhiroServer.Controllers.AccesoController;
using JhiroServer.Models;
public class PedidoService
{
    private readonly HttpClient _httpClient;

    public PedidoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PedidoResponse> RealizarPedido(PedidoRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("https://jhiro.somee.com/api/Acceso/RealizarPedido", request);
        return await response.Content.ReadFromJsonAsync<PedidoResponse>();
    }
}
