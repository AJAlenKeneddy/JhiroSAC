using JhiroServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace JhiroServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagoController : ControllerBase
    {
        private readonly JhiroTiendaDBContext _context;

        public PagoController(JhiroTiendaDBContext context)
        {
            _context = context;
        }

        // POST: api/Pago/realizarPago
        [HttpPost("realizarPago")]
        public async Task<ActionResult<Response<string>>> RealizarPago([FromBody] PagoRequest request)
        {
            try
            {
                // Verificar si el carrito es válido y contiene productos
                if (request == null || request.Productos == null || request.Productos.Count == 0)
                {
                    return BadRequest(new Response<string> { IsSuccess = false, Message = "El carrito está vacío o la solicitud es inválida." });
                }

                // Verificar si se proporcionó el correo electrónico
                if (string.IsNullOrEmpty(request.CorreoElectronico))
                {
                    return BadRequest(new Response<string> { IsSuccess = false, Message = "Correo electrónico no proporcionado." });
                }

                // Crear la orden de pago en la base de datos utilizando los modelos existentes
                var nuevaOrden = new Orden
                {
                    CorreoElectronico = request.CorreoElectronico,
                    FechaCreacion = DateTime.Now,
                    Total = request.Productos.Sum(p => p.Cantidad * p.Precio),
                    OrdenProductos = request.Productos.Select(p => new OrdenProducto
                    {
                        ProductoId = p.ProductoId,
                        Cantidad = p.Cantidad,
                        Precio = p.Precio
                    }).ToList()
                };

                _context.Ordens.Add(nuevaOrden);
                await _context.SaveChangesAsync();

                return Ok(new Response<string>
                {
                    IsSuccess = true,
                    Message = "El pago se ha realizado exitosamente y la orden ha sido creada.",
                    Data = nuevaOrden.Id.ToString() // Retornamos el ID de la orden creada
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string> { IsSuccess = false, Message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        public class PagoRequest
        {
            public string CorreoElectronico { get; set; }
            public List<ProductoPagoDTO> Productos { get; set; }
        }

        public class ProductoPagoDTO
        {
            public int ProductoId { get; set; }
            public string NombreProducto { get; set; }
            public int Cantidad { get; set; }
            public decimal Precio { get; set; }
        }
    }
}
