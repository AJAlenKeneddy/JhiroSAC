using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using JhiroServer.Models;

namespace JhiroServer.Services
{
    public class PagoService
    {
        private readonly HttpClient _httpClient;

        public PagoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para realizar el pago
        public async Task<Response<string>> RealizarPago(PagoRequest pagoRequest)
        {
            try
            {
                // Enviar la solicitud POST al endpoint "realizarPago"
                var response = await _httpClient.PostAsJsonAsync("api/Pago/realizarPago", pagoRequest);

                // Verificar si la solicitud fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Leer la respuesta del servidor
                    var resultado = await response.Content.ReadFromJsonAsync<Response<string>>();
                    return resultado;
                }
                else
                {
                    // Manejar errores de la solicitud
                    return new Response<string>
                    {
                        IsSuccess = false,
                        Message = $"Error: {response.ReasonPhrase}"
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                // Manejar excepciones de red o problemas con la solicitud
                return new Response<string>
                {
                    IsSuccess = false,
                    Message = $"Error al conectar con el servidor: {ex.Message}"
                };
            }
        }
    }

    // PagoRequest se utiliza para enviar la solicitud de pago
    public class PagoRequest
    {
        public string CorreoElectronico { get; set; }
        public List<ProductoPagoDTO> Productos { get; set; }
    }

    // DTO para los productos que forman parte del pago
    public class ProductoPagoDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }

  
    
}
