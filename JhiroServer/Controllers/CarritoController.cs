using JhiroServer.Models;
using JhiroServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace JhiroServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly JhiroTiendaDBContext _context;
        private readonly IJwtService _jwtService;

        public CarritoController(JhiroTiendaDBContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet("obtenerCarrito")]
        public async Task<ActionResult<Response<IEnumerable<CarritoSPResult>>>> GetCarrito()
        {
            try
            {
                
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new Response<IEnumerable<CarritoSPResult>> { IsSuccess = false, Message = "Token no proporcionado." });
                }

               
                var userIdString = _jwtService.GetUserIdFromToken(token);

                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized(new Response<IEnumerable<CarritoSPResult>> { IsSuccess = false, Message = "Token inválido o no se pudo obtener el ID de usuario." });
                }

                
                if (!int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(new Response<IEnumerable<CarritoSPResult>> { IsSuccess = false, Message = "El ID de usuario no es válido." });
                }

                var query = "EXEC ObtenerCarrito @UserId";
                var carritos = await _context.CarritoSPResults
                    .FromSqlRaw(query, new SqlParameter("@UserId", userId))
                    .AsNoTracking() 
                    .ToListAsync();


                
                var response = new Response<IEnumerable<CarritoSPResult>>
                {
                    IsSuccess = true,
                    Data = carritos
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<IEnumerable<CarritoSPResult>> { IsSuccess = false, Message = $"Error interno del servidor: {ex.Message}" });
            }
        }




        [HttpPost("agregarAlCarrito")]
        public async Task<ActionResult<Response<string>>> AddToCarrito([FromBody] AddToCarritoRequest request)
        {
            try
            {
                
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "Token no proporcionado." });
                }

                
                var userIdString = _jwtService.GetUserIdFromToken(token);

                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "Token inválido o no se pudo obtener el ID de usuario." });
                }

                
                if (!int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "El ID de usuario no es válido." });
                }

                
                var existingItem = await _context.Carritos
                    .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.ProductoId == request.ProductoId);

                if (existingItem != null)
                {
                    
                    existingItem.Cantidad += request.Cantidad;
                    _context.Carritos.Update(existingItem);
                }
                else
                {
                    
                    var nuevoItem = new Carrito
                    {
                        UsuarioId = userId,
                        ProductoId = request.ProductoId,
                        Cantidad = request.Cantidad
                    };
                    await _context.Carritos.AddAsync(nuevoItem);
                }

                await _context.SaveChangesAsync();

                return Ok(new Response<string> { IsSuccess = true, Message = "Producto agregado al carrito exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string> { IsSuccess = false, Message = $"Error interno del servidor: {ex.Message}" });
            }
        }



        [HttpPut("actualizarCarrito")]
        public async Task<ActionResult<Response<string>>> ActualizarCantidad([FromBody] AddToCarritoRequest request)
        {
            try
            {
                
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "Token no proporcionado." });
                }

                
                var userIdString = _jwtService.GetUserIdFromToken(token);

                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "Token inválido o no se pudo obtener el ID de usuario." });
                }

               
                if (!int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "El ID de usuario no es válido." });
                }

                var carritoItem = await _context.Carritos
                    .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.ProductoId == request.ProductoId);

                if (carritoItem == null)
                {
                    return NotFound(new Response<string> { IsSuccess = false, Message = "Producto no encontrado en el carrito." });
                }

               
                carritoItem.Cantidad = request.Cantidad;
                _context.Carritos.Update(carritoItem);

                await _context.SaveChangesAsync();

                return Ok(new Response<string> { IsSuccess = true, Message = "Cantidad actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string> { IsSuccess = false, Message = $"Error interno del servidor: {ex.Message}" });
            }
        }


        [HttpDelete("eliminarDelCarrito/{productoId}")]
        public async Task<ActionResult<Response<string>>> EliminarProductoDelCarrito(int productoId)
        {
            try
            {
                
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "Token no proporcionado." });
                }

                
                var userIdString = _jwtService.GetUserIdFromToken(token);

                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "Token inválido o no se pudo obtener el ID de usuario." });
                }

                
                if (!int.TryParse(userIdString, out int userId))
                {
                    return Unauthorized(new Response<string> { IsSuccess = false, Message = "El ID de usuario no es válido." });
                }

                
                var result = _context.Set<EliminarProductoCarritoResult>()
                    .FromSqlRaw("EXEC EliminarProductoDelCarrito @UserId, @ProductoId",
                        new SqlParameter("@UserId", userId),
                        new SqlParameter("@ProductoId", productoId))
                    .AsEnumerable()
                    .FirstOrDefault();

                if (result == null)
                {
                    return NotFound(new Response<string> { IsSuccess = false, Message = "Producto no encontrado en el carrito." });
                }

                return Ok(new Response<string> { IsSuccess = true, Message = result.Mensaje });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string> { IsSuccess = false, Message = $"Error interno del servidor: {ex.Message}" });
            }
        }





    }
}
