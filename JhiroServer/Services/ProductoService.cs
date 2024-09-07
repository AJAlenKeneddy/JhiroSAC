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
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Producto>>("http://Jhiro.somee.com/api/Productoes");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
                return new List<Producto>(); // Retornar lista vacía en caso de error
            }
        }

        public async Task<List<Producto>> GetProductosPorCategoriaAsync(int categoriaId, int pageNumber)
        {
            try
            {
                var url = $"http://Jhiro.somee.com/api/Productoes/GetProductosPorCategoria?categoriaId={categoriaId}&pageNumber={pageNumber}";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Lanza una excepción si el código de estado HTTP no es exitoso
                return await response.Content.ReadFromJsonAsync<List<Producto>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos por categoría: {ex.Message}");
                return new List<Producto>(); // Retornar lista vacía en caso de error
            }
        }

        public async Task<Categoria> GetCategoriaAsync(int categoriaId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Categoria>($"http://Jhiro.somee.com/api/Categorias/Categoria/{categoriaId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener categoría: {ex.Message}");
                return null; // Retornar null en caso de error
            }
        }
    }
}
