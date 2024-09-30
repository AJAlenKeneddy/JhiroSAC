using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JhiroServer.Models;
using Microsoft.EntityFrameworkCore;
using JhiroServer.Custom;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using JhiroServer.Services;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;



namespace JhiroServer.Controllers
{
    /* 
     Configuracion del Enpoint y llamado de Contexto de la BD ademas 
     de Utilidades y el Servicio de Envio de Correo
    */
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly JhiroTiendaDBContext db;
        private readonly Utilidades util;
        private readonly EmailService emailService;
        private readonly IJwtService _jwtService;
        private readonly HttpClient _httpClient;
        private readonly PayPalService payPalService;
        public AccesoController(JhiroTiendaDBContext jhiroTiendaDBContext, Utilidades utilidades, EmailService emailService, IJwtService jwtService, HttpClient httpClient, PayPalService payPalService)
        {
            db = jhiroTiendaDBContext;
            util = utilidades;
            this.emailService = emailService;
            _jwtService = jwtService;
            _httpClient = httpClient;
            this.payPalService = payPalService;
        }





        /*
         Endpoint para Registrar Usuarios ademas de generacion de un codigo de Verificacion de 
         5 digitos y su envio a su respectivo correo el cual hace uso de Utilidades de Hash Contraseña y EmailService 
        */
        [HttpPost]
        [Route("Registrarse")]
        public async Task<IActionResult> Registrarse([FromBody] RegistrarseRequest request)
        {
            // Validación de campos requeridos
            if (request == null || string.IsNullOrEmpty(request.Correo))
                return BadRequest(new RegistrarseResponse { IsSuccess = false, Message = "Todos los campos son requeridos." });

            try
            {
                // Validar si el correo ya está en uso antes de insertar
                var usuarioExistente = await db.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.Correo);
                if (usuarioExistente != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict, new
                    {
                        IsSuccess = false,
                        Message = "El correo electrónico ya está en uso."
                    });
                }





                var modeloUsuario = new Usuario
                {
                    Correo = request.Correo

                };


                await db.Usuarios.AddAsync(modeloUsuario);
                await db.SaveChangesAsync();

                // Enviar correo electrónico con el código de verificación
                var emailBody = $@"
                <html>
                <body>
                    <h2>Verificación de Correo</h2>
                    <p>Tu código de verificación es:
                    <br>
                    <strong></strong></p>
                </body>
                </html>";

                var emailSent = await emailService.SendEmailAsync(
                    "Remitente",
                    "alenaguilar24@gmail.com",
                    "Destinatario",
                    request.Correo,
                    "Código de Verificación",
                    emailBody);

                // Verificar si el correo fue enviado con éxito
                if (emailSent)
                    return Ok(new RegistrarseResponse { IsSuccess = true, Message = "Registro exitoso. Verifica tu correo electrónico." });
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, new RegistrarseResponse { IsSuccess = false, Message = "Error al enviar el correo. Inténtalo de nuevo." });
                return Ok(new RegistrarseResponse { IsSuccess = true, Message = "Registro exitoso. Verifica tu correo electrónico." });
            }
            catch (DbUpdateException dbEx)
            {
                // Manejo de errores de base de datos
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Error al guardar los cambios",
                    Details = dbEx.Message
                });
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    IsSuccess = false,
                    Message = "Error interno del servidor.",
                    Details = ex.Message
                });
            }
        }








        [HttpPost]
        [Route("RealizarPedido")]
        public async Task<IActionResult> RealizarPedido([FromBody] PedidoRequest request)
        {
            // Validación de campos requeridos
            if (request == null || string.IsNullOrEmpty(request.Correo))
                return BadRequest(new PedidoResponse { IsSuccess = false, Message = "Todos los campos son requeridos." });

            try
            {
                // Validar si el correo ya está en uso antes de insertar
                var usuarioExistente = await db.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.Correo);
                if (usuarioExistente == null)
                {
                    var nuevoUsuario = new Usuario
                    {
                        Correo = request.Correo
                    };

                    await db.Usuarios.AddAsync(nuevoUsuario);
                    await db.SaveChangesAsync();
                }

                // Construir la lista de productos con imagen, nombre, cantidad y precio
                var productosHtml = "";
                foreach (var producto in request.Productos)
                {
                    productosHtml += $@"
                    <tr>
                        <td style='padding: 10px; border-bottom: 1px solid #eee;'>
                            <img src='{producto.ImagenUrl}' alt='{producto.NombreProducto}' style='max-width: 100px; max-height: 100px;'/>
                        </td>
                        <td style='padding: 10px; border-bottom: 1px solid #eee;'>
                            <strong style='font-size: 16px; color: #333;'>{producto.NombreProducto}</strong><br/>
                            <span style='font-size: 14px; color: #666;'>Cantidad: {producto.Cantidad}</span><br/>
                            <span style='font-size: 14px; color: #666;'>Precio: {producto.Precio.ToString("C")}</span>
                        </td>
                    </tr>";
                }

                var emailBody = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            padding: 20px;
                        }}
                        .card {{
                            background-color: #fff;
                            border-radius: 8px;
                            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                            padding: 20px;
                            max-width: 600px;
                            margin: 0 auto;
                        }}
                        h2 {{
                            color: #333;
                            font-size: 24px;
                            margin-bottom: 10px;
                        }}
                        p {{
                            color: #666;
                            font-size: 16px;
                            margin: 5px 0;
                        }}
                        .footer {{
                            margin-top: 20px;
                            text-align: center;
                            font-size: 14px;
                            color: #888;
                        }}
                    </style>
                </head>
                <body>
                    <div class='card'>
                        <h2>Detalles de tu Pedido</h2>
                        <p>Has realizado un pedido con los siguientes productos:</p>
                        <table style='width: 100%; border-collapse: collapse;'>
                            {productosHtml}
                        </table>
                        <div class='footer'>
                            <p>Gracias por tu compra. ¡Esperamos que disfrutes tus productos!</p>
                        </div>
                    </div>
                </body>
                </html>";


                // Enviar el correo electrónico
                var emailSent = await emailService.SendEmailAsync(
                    "Remitente",
                    "alenaguilar24@gmail.com",
                    "Destinatario",
                    request.Correo,
                    "Detalles de tu Pedido",
                    emailBody);

                if (emailSent)
                    return Ok(new PedidoResponse { IsSuccess = true, Message = "Pedido realizado con éxito. Verifica tu correo electrónico para los detalles." });
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, new PedidoResponse { IsSuccess = false, Message = "Error al enviar el correo. Inténtalo de nuevo." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new PedidoResponse
                {
                    IsSuccess = false,
                    Message = "Error interno del servidor.",
                    Details = ex.Message
                });
            }
        }






        /*
         Endpoint para poder Iniciar Session con utilidad de DesHash de Contraseña     
         */
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Correo))
                return BadRequest(new LoginResponse { IsSuccess = false, Message = "Correo es requerido." });

            try
            {
                var usuarioEncontrado = await db.Usuarios
                    .Where(u => u.Correo == loginRequest.Correo)
                    .FirstOrDefaultAsync();

                if (usuarioEncontrado == null)
                    return Unauthorized(new LoginResponse { IsSuccess = false, Message = "Credenciales inválidas." });

                var token = _jwtService.GenerateToken(usuarioEncontrado);
                var userId = _jwtService.GetUserIdFromToken(token); // Obtener el UserId desde el token

                return Ok(new LoginResponse
                {
                    IsSuccess = true,
                    Token = token,
                    UserId = userId,
                    Message = "Inicio de sesión exitoso."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new LoginResponse
                {
                    IsSuccess = false,
                    Message = "Error interno del servidor."
                });
            }
        }



        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("CerrarSesion")]
        public IActionResult CerrarSesion()
        {

            return Ok(new { IsSuccess = true, Message = "Sesión cerrada exitosamente." });
        }


    }
}