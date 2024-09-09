using System;
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

        // Obtener todos los productos
        public async Task<List<Producto>> GetProductosAsync()
        {
            try
            {
                var productos = await _httpClient.GetFromJsonAsync<List<Producto>>("https://jhiro.somee.com/api/Productoes/obtenerProductos");
                return productos ?? new List<Producto>(); // Retorna lista vacía si es null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos: {ex.Message}");
                return new List<Producto>(); // Retornar lista vacía en caso de error
            }
        }


        // Obtener productos por categoría
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

        // Obtener una categoría por su ID
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

        // Obtener un producto por su ID
        public async Task<Producto> GetProductoByIdAsync(int productoId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Producto>($"https://jhiro.somee.com/api/Productoes/UnProducto/{productoId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el producto: {ex.Message}");
                return null; // Retornar null en caso de error
            }
        }

        // Crear un nuevo producto
        public async Task<bool> CreateProductoAsync(Producto nuevoProducto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://jhiro.somee.com/api/Productoes/CrearProducto", nuevoProducto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear el producto: {ex.Message}");
                return false; // Retornar false en caso de error
            }
        }

        // Actualizar un producto existente
        public async Task<bool> UpdateProductoAsync(Producto productoActualizado)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"https://jhiro.somee.com/api/Productoes/ActualizarProducto/{productoActualizado.ProductoId}", productoActualizado);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el producto: {ex.Message}");
                return false; // Retornar false en caso de error
            }
        }

        // Eliminar un producto por su ID
        public async Task<bool> DeleteProductoAsync(int productoId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"https://jhiro.somee.com/api/Productoes/EliminarProducto/{productoId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el producto: {ex.Message}");
                return false; // Retornar false en caso de error
            }
        }

        public async Task<List<Producto>> GetProductosEliminadosAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Producto>>("https://jhiro.somee.com/api/Productoes/obtenerProductosEliminados");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener productos eliminados: {ex.Message}");
                return new List<Producto>(); // Retornar lista vacía en caso de error
            }
        }

        public async Task<bool> RestaurarProductoAsync(int productoId)
        {
            try
            {
                var response = await _httpClient.PostAsync($"https://jhiro.somee.com/api/Productoes/RestaurarProducto/{productoId}", null);
                response.EnsureSuccessStatusCode(); 
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al restaurar producto: {ex.Message}");
                return false; // Retornar false en caso de error
            }
        }
        public async Task<Producto> GetProductoEliminadoAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Producto>($"https://jhiro.somee.com/api/Productoes/UnProductoEliminado/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener producto eliminado: {ex.Message}");
                return null; // Retornar null en caso de error
            }
        }

    }
}
