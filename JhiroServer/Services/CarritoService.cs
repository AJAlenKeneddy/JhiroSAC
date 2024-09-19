using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using JhiroServer.Models;
using JhiroServer.Services;
using Microsoft.JSInterop;

public class CarritoService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly IJwtService _jwtService;

    public CarritoService(HttpClient httpClient, IJSRuntime jsRuntime, IJwtService jwtService)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _jwtService = jwtService;
    }

    public async Task<bool> AddToCarrito(int productoId, int cantidad)
    {
        try
        {
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            
            var request = new AddToCarritoRequest
            {
                ProductoId = productoId,
                Cantidad = cantidad
            };

         
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7048/api/Carrito/agregarAlCarrito", request);

            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<string>>();
                return result?.IsSuccess ?? false;
            }
            else
            {
              
                Console.Error.WriteLine($"Error al agregar producto al carrito: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            
            Console.Error.WriteLine($"Error inesperado al agregar producto al carrito: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<CarritoDTO>> GetCarritoByUsuarioId()
    {
        try
        {
            
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

           
            var response = await _httpClient.GetAsync("https://localhost:7048/api/Carrito/obtenerCarrito");

            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<CarritoDTO>>>();
                return result?.Data ?? Enumerable.Empty<CarritoDTO>();
            }
            else
            {
              
                Console.Error.WriteLine($"Error al obtener el carrito: {response.ReasonPhrase}");
                return Enumerable.Empty<CarritoDTO>();
            }
        }
        catch (HttpRequestException ex)
        {
            
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return Enumerable.Empty<CarritoDTO>();
        }
        catch (Exception ex)
        {
            
            Console.Error.WriteLine($"Error inesperado al obtener el carrito: {ex.Message}");
            return Enumerable.Empty<CarritoDTO>();
        }
    }

    public async Task<bool> ActualizarCantidad(int productoId, int nuevaCantidad)
    {
        try
        {
            
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            
            var request = new AddToCarritoRequest
            {
                ProductoId = productoId,
                Cantidad = nuevaCantidad
            };

            
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7048/api/Carrito/actualizarCarrito", request);

            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<string>>();
                return result?.IsSuccess ?? false;
            }
            else
            {
               
                Console.Error.WriteLine($"Error al actualizar la cantidad del producto: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
           
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
           
            Console.Error.WriteLine($"Error inesperado al actualizar la cantidad del producto: {ex.Message}");
            return false;
        }

    }
    public async Task<bool> EliminarProductoDelCarrito(int productoId)
    {
        try
        {
            
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"https://localhost:7048/api/Carrito/eliminarDelCarrito/{productoId}");

            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<string>>();
                return result?.IsSuccess ?? false;
            }
            else
            {
                
                Console.Error.WriteLine($"Error al eliminar producto del carrito: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
           
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            
            Console.Error.WriteLine($"Error inesperado al eliminar producto del carrito: {ex.Message}");
            return false;
        }
    }

}
