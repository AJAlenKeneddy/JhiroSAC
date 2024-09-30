using System.Net.Http.Headers;
using JhiroServer.Models;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks; // Asegúrate de importar este espacio de nombres

namespace JhiroServer.Services
{
    public class PayPalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _clientId;
        private readonly string _secret;
        private readonly string _paypalApiUrl = "https://api-m.sandbox.paypal.com";

        public PayPalService(HttpClient httpClient, PayPalOptions payPalOptions)
        {
            _httpClient = httpClient;
            _clientId = payPalOptions.ClientId;
            _secret = payPalOptions.ClientSecret;
        }

        public async Task<(string approveUrl, string token)> CreatePayPalOrder(decimal amount)
        {
            Console.WriteLine($"Creando orden para el monto: {amount:F2}"); // Usa la interpolación de cadenas
            var accessToken = await GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var orderRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = amount.ToString("F2") // Asegúrate de que el formato sea correcto
                        }
                    }
                },
                application_context = new
                {
                    return_url = "https://localhost:7048/pedido?status=success",
                    cancel_url = "https://localhost:7048/pedido?status=cancelled"
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(orderRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_paypalApiUrl}/v2/checkout/orders", content);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
                var approveUrl = jsonResponse.GetProperty("links").EnumerateArray()
                    .First(link => link.GetProperty("rel").GetString() == "approve")
                    .GetProperty("href").GetString();

                var token = jsonResponse.GetProperty("id").GetString(); // Obtener el token de la respuesta
                Console.WriteLine("Orden creada con éxito. URL de aprobación: " + approveUrl);
                return (approveUrl, token);
            }
            else
            {
                Console.WriteLine("Error al crear la orden de PayPal: " + responseString);
                throw new Exception("Error al crear la orden de PayPal: " + responseString);
            }
        }

        public async Task<CaptureResponse> CapturePayPalOrder(string token)
        {
            Console.WriteLine("Capturando pago de PayPal...");
            var accessToken = await GetAccessToken();
            var captureUrl = $"{_paypalApiUrl}/v2/checkout/orders/{token}/capture";
            var request = new HttpRequestMessage(HttpMethod.Post, captureUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Cargar la información para capturar el pago
            var captureRequest = new
            {
                // Cambia el valor aquí para usar el monto real que quieres capturar
                amount = new
                {
                    currency_code = "USD",
                    value = "225.00" // Asegúrate de usar el valor correcto aquí
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(captureRequest), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Pago capturado exitosamente.");
                return new CaptureResponse
                {
                    IsSuccess = true,
                    Message = "Pago capturado exitosamente."
                    // Puedes agregar el ID de la transacción si lo recibes en la respuesta
                };
            }
            else
            {
                Console.WriteLine($"Error al capturar el pago: {responseString}");
                return new CaptureResponse
                {
                    IsSuccess = false,
                    Message = $"Error al capturar el pago: {responseString}"
                };
            }
        }

        private async Task<string> GetAccessToken()
        {
            Console.WriteLine("Obteniendo token de acceso...");
            var byteArray = Encoding.ASCII.GetBytes($"{_clientId}:{_secret}");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var response = await _httpClient.PostAsync($"{_paypalApiUrl}/v1/oauth2/token", new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded"));
            var responseString = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = JsonSerializer.Deserialize<JsonElement>(responseString);
                var accessToken = jsonResponse.GetProperty("access_token").GetString();
                Console.WriteLine("Token de acceso obtenido con éxito.");
                return accessToken;
            }
            else
            {
                Console.WriteLine("Error al obtener el token de acceso: " + responseString);
                throw new Exception("Error al obtener el token de acceso: " + responseString);
            }
        }

        public class CaptureResponse
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
            public string TransactionId { get; set; } // Opcional: puedes incluir el ID de transacción de PayPal si lo necesitas
        }
    }
}
