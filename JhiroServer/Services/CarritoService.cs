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
            // Obtén el token de autenticación desde localStorage
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            // Configura el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Crea la solicitud para agregar el producto al carrito
            var request = new AddToCarritoRequest
            {
                ProductoId = productoId,
                Cantidad = cantidad
            };

            // Realiza la solicitud POST al endpoint del carrito
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7048/api/Carrito/agregarAlCarrito", request);

            // Verifica la respuesta y devuelve true si la operación fue exitosa
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<string>>();
                return result?.IsSuccess ?? false;
            }
            else
            {
                // Manejar el error (ej. notificar al usuario)
                Console.Error.WriteLine($"Error al agregar producto al carrito: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            // Loguea el error y retorna false en caso de problemas con la solicitud
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Loguea cualquier error inesperado y retorna false
            Console.Error.WriteLine($"Error inesperado al agregar producto al carrito: {ex.Message}");
            return false;
        }
    }

    public async Task<IEnumerable<CarritoDTO>> GetCarritoByUsuarioId()
    {
        try
        {
            // Obtén el token de autenticación desde localStorage
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            // Configura el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Realiza la solicitud GET al endpoint del carrito
            var response = await _httpClient.GetAsync("https://localhost:7048/api/Carrito/obtenerCarrito");

            // Verifica la respuesta y retorna la lista de items en el carrito
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<IEnumerable<CarritoDTO>>>();
                return result?.Data ?? Enumerable.Empty<CarritoDTO>();
            }
            else
            {
                // Manejar el error (ej. notificar al usuario)
                Console.Error.WriteLine($"Error al obtener el carrito: {response.ReasonPhrase}");
                return Enumerable.Empty<CarritoDTO>();
            }
        }
        catch (HttpRequestException ex)
        {
            // Loguea el error y retorna una lista vacía en caso de problemas con la solicitud
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return Enumerable.Empty<CarritoDTO>();
        }
        catch (Exception ex)
        {
            // Loguea cualquier error inesperado y retorna una lista vacía
            Console.Error.WriteLine($"Error inesperado al obtener el carrito: {ex.Message}");
            return Enumerable.Empty<CarritoDTO>();
        }
    }

    public async Task<bool> ActualizarCantidad(int productoId, int nuevaCantidad)
    {
        try
        {
            // Obtén el token de autenticación desde localStorage
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            // Configura el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Crea la solicitud para actualizar la cantidad del producto en el carrito
            var request = new AddToCarritoRequest
            {
                ProductoId = productoId,
                Cantidad = nuevaCantidad
            };

            // Realiza la solicitud PUT al endpoint del carrito
            var response = await _httpClient.PutAsJsonAsync("https://localhost:7048/api/Carrito/actualizarCarrito", request);

            // Verifica la respuesta y devuelve true si la operación fue exitosa
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<string>>();
                return result?.IsSuccess ?? false;
            }
            else
            {
                // Manejar el error (ej. notificar al usuario)
                Console.Error.WriteLine($"Error al actualizar la cantidad del producto: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            // Loguea el error y retorna false en caso de problemas con la solicitud
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Loguea cualquier error inesperado y retorna false
            Console.Error.WriteLine($"Error inesperado al actualizar la cantidad del producto: {ex.Message}");
            return false;
        }

    }
    public async Task<bool> EliminarProductoDelCarrito(int productoId)
    {
        try
        {
            // Obtén el token de autenticación desde localStorage
            var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "authToken");

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No se encontró el token de autenticación.");
            }

            // Configura el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Crea la solicitud para eliminar el producto del carrito
            var response = await _httpClient.DeleteAsync($"https://localhost:7048/api/Carrito/eliminarDelCarrito/{productoId}");

            // Verifica la respuesta y devuelve true si la operación fue exitosa
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Response<string>>();
                return result?.IsSuccess ?? false;
            }
            else
            {
                // Manejar el error (ej. notificar al usuario)
                Console.Error.WriteLine($"Error al eliminar producto del carrito: {response.ReasonPhrase}");
                return false;
            }
        }
        catch (HttpRequestException ex)
        {
            // Loguea el error y retorna false en caso de problemas con la solicitud
            Console.Error.WriteLine($"Error al realizar la solicitud: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            // Loguea cualquier error inesperado y retorna false
            Console.Error.WriteLine($"Error inesperado al eliminar producto del carrito: {ex.Message}");
            return false;
        }
    }

}
