using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JhiroServer.Models;

namespace JhiroServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoesController : ControllerBase
    {
        private readonly JhiroTiendaDBContext _context;

        public ProductoesController(JhiroTiendaDBContext context)
        {
            _context = context;
        }

        // GET: api/Productoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }

            // Filtrar por productos que no han sido eliminados lógicamente
            return await _context.Productos
                                .Where(p => !p.Eliminado)
                                .ToListAsync();
        }

        // GET: api/Productoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }

            // Buscar producto por ID y verificar que no esté eliminado
            var producto = await _context.Productos
                                          .Where(p => p.ProductoId == id && !p.Eliminado)
                                          .FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // PUT: api/Productoes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, Producto producto)
        {
            if (id != producto.ProductoId)
            {
                return BadRequest();
            }

            var existingProducto = await _context.Productos.FindAsync(id);
            if (existingProducto == null || existingProducto.Eliminado)
            {
                return NotFound();
            }

            // Actualizar el producto
            existingProducto.Nombre = producto.Nombre;
            existingProducto.Descripcion = producto.Descripcion;
            existingProducto.Precio = producto.Precio;
            existingProducto.Stock = producto.Stock;
            existingProducto.ImagenUrl = producto.ImagenUrl;
            existingProducto.CategoriaId = producto.CategoriaId;
            existingProducto.Eliminado = producto.Eliminado;

            _context.Entry(existingProducto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Productoes
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'JhiroTiendaDBContext.Productos' is null.");
            }

            // Asegurar que el producto se crea como no eliminado
            producto.Eliminado = false;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.ProductoId }, producto);
        }

        // DELETE: api/Productoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null || producto.Eliminado)
            {
                return NotFound();
            }

            // Eliminación lógica en lugar de eliminación física
            producto.Eliminado = true;

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Productoes/restore/5
        [HttpPost("restore/{id}")]
        public async Task<IActionResult> RestoreProducto(int id)
        {
            if (_context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            // Restaurar el producto eliminado lógicamente
            if (!producto.Eliminado)
            {
                return BadRequest("El producto no está eliminado.");
            }

            producto.Eliminado = false;
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return (_context.Productos?.Any(e => e.ProductoId == id && !e.Eliminado)).GetValueOrDefault();
        }
    }
}
