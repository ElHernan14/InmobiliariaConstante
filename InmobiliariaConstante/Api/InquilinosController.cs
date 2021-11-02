using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InmobiliariaConstante.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace InmobiliariaConstante.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InquilinosController : ControllerBase
    {
        private readonly DataContext _context;

        public InquilinosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Inquilinoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inquilino>>> GetInquilinos()
        {
            return await _context.Inquilinos.ToListAsync();
        }

        // GET: api/Inquilinoes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inquilino>> GetInquilino(int id)
        {
            var inquilino = await _context.Inquilinos.FindAsync(id);

            if (inquilino == null)
            {
                return NotFound();
            }

            return inquilino;
        }

        // PUT: api/Inquilinoes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInquilino(int id, Inquilino inquilino)
        {
            if (id != inquilino.IdInquilino)
            {
                return BadRequest();
            }

            _context.Entry(inquilino).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InquilinoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(inquilino);
        }

        // POST: api/Inquilinoes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inquilino>> PostInquilino(Inquilino inquilino)
        {
            try
            {
                _context.Inquilinos.Add(inquilino);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetInquilino", new { id = inquilino.IdInquilino }, inquilino);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Inquilinoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInquilino(int id)
        {
            var inquilino = await _context.Inquilinos.FindAsync(id);
            if (inquilino == null)
            {
                return NotFound();
            }

            _context.Inquilinos.Remove(inquilino);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool InquilinoExists(int id)
        {
            return _context.Inquilinos.Any(e => e.IdInquilino == id);
        }

        [HttpGet("contratos")]
        public async Task<ActionResult<IEnumerable<Inmueble>>> obtenerInmuebleContratoPropietario()
        {
            try
            {
                var usuario = User.Identity.Name;
                var inmueblesConContrato = _context.Contrato.Include(i => i.inmueble).Where(i => i.inmueble.Duenio.Email == usuario).
                    Select(i => i.inmueble);
                return Ok(inmueblesConContrato);    
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("inqui/{id}")]
        public async Task<ActionResult<Inquilino>>obtenerInquilinoXInmueble(int id)
        {
            try
            {
                var usuario = User.Identity.Name;
                var inquilinos = _context.Contrato.Include(x => x.inmueble).Where(x => x.inmueble.Id == id).Select(i => i.inquilino).FirstOrDefault();
                    
                return Ok(inquilinos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
