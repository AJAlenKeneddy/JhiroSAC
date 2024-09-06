using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Back.Models;

namespace Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly JhiroTiendaDBContext _context;

        public CategoriasController(JhiroTiendaDBContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [HttpGet("ListadoCategorias")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }

            // Filtrar por las categorías que no han sido eliminadas lógicamente
            return await _context.Categorias
                                .Where(c => c.Eliminado == false || c.Eliminado == null)
                                .ToListAsync();
        }

        // GET: api/Categorias/5
        [HttpGet("Categoria/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }
            var categoria = await _context.Categorias
                                          .Where(c => c.Eliminado == false && c.CategoriaId == id)
                                          .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // PUT: api/Categorias/5
        [HttpPut("ActualizarCategoria/{id}")]
        public async Task<IActionResult> PutCategoria(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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

        // POST: api/Categorias
        [HttpPost("CrearCategoria")]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'JhiroTiendaDBContext.Categorias'  is null.");
            }

            categoria.Eliminado = false; // Asegurar que la categoría se crea como no eliminada
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        // DELETE: api/Categorias/5
        [HttpDelete("EliminarCategoria/{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            // Eliminación lógica en lugar de eliminación física
            categoria.Eliminado = true;

            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // RESTORE: api/Categorias/5
        [HttpPut("RestaurarCategoria/{id}")]
        public async Task<IActionResult> RestaurarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null || categoria.Eliminado == false)
            {
                return NotFound();
            }

            // Restaurar la categoría
            categoria.Eliminado = false;
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return (_context.Categorias?.Any(e => e.CategoriaId == id)).GetValueOrDefault();
        }
    }
}
