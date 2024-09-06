using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using JhiroServer.Models;

namespace JhiroServer.Services
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Producto>>("https://localhost:7048/api/Productoes");
        }
    }
}
