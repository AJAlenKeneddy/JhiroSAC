﻿using System;
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
    public class CategoriasController : ControllerBase
    {
        private readonly JhiroTiendaDBContext _context;

        public CategoriasController(JhiroTiendaDBContext context)
        {
            _context = context;
        }

        [HttpGet("ListadoCategorias")]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            if (_context.Categorias == null)
            {
                return NotFound();
            }

            
            return await _context.Categorias
                                .Where(c => c.Eliminado == false || c.Eliminado == null)
                                .ToListAsync();
        }

        
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

        
        [HttpPost("CrearCategoria")]
        public async Task<ActionResult<Categoria>> PostCategoria(Categoria categoria)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'JhiroTiendaDBContext.Categorias'  is null.");
            }

            categoria.Eliminado = false;
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        
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

           
            categoria.Eliminado = true;

            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        
        [HttpPut("RestaurarCategoria/{id}")]
        public async Task<IActionResult> RestaurarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null || categoria.Eliminado == false)
            {
                return NotFound();
            }

            
            categoria.Eliminado = false;
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("NombreCategorias/{id}")]
        public async Task<ActionResult<Categoria>> GetCategoriaNombre(int id)
        {
            var categoria = await _context.Categorias
                                           .Where(c => c.CategoriaId == id)
                                           .Select(c => new Categoria
                                           {
                                               Nombre = c.Nombre,
                                               Publicidad = c.Publicidad
                                           })
                                           .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound("Categoría no encontrada.");
            }

            return Ok(categoria);
        }


        private bool CategoriaExists(int id)
        {
            return (_context.Categorias?.Any(e => e.CategoriaId == id)).GetValueOrDefault();
        }
    }
}
