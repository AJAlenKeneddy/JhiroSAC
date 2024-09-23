using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json.Nodes;

namespace JhiroServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckoutController : Controller
    {
        private string PaypalClientId { get; set; }
        private string PaypalSecret { get; set; }
        private string PaypalUrl { get; set; }

        public CheckoutController(IConfiguration configuration)
        {
            PaypalClientId = configuration["PayPalSettings:ClientId"]!;
            PaypalSecret = configuration["PayPalSettings:Secret"]!;
            PaypalUrl = configuration["PayPalSettings:Url"]!;
        }

        // Método para devolver el ClientId de PayPal
        [HttpGet("clientid")]
        public IActionResult GetPaypalClientId()
        {
            return Ok(new { clientId = PaypalClientId });
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetPaypalToken()
        {
            var token = await GetPaypalAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(500, "Error retrieving PayPal access token.");
            }
            return Ok(new { access_token = token });
        }

        private async Task<string> GetPaypalAccessToken()
        {
            string accessToken = "";
            string url = PaypalUrl + "/v1/oauth2/token";

            using (var client = new HttpClient())
            {
                string credentials64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(PaypalClientId + ":" + PaypalSecret));

                client.DefaultRequestHeaders.Add("Authorization", "Basic " + credentials64);

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
                requestMessage.Content = new StringContent("grant_type=client_credentials", null, "application/x-www-form-urlencoded");

                var httpResponse = await client.SendAsync(requestMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var strResponse = await httpResponse.Content.ReadAsStringAsync();
                    var jsonResponse = JsonNode.Parse(strResponse);
                    if (jsonResponse != null)
                    {
                        accessToken = jsonResponse["access_token"]?.ToString() ?? "";
                    }
                }
            }

            return accessToken;
        }
    }
}
