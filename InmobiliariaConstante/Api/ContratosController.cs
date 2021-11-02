using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InmobiliariaConstante.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace InmobiliariaConstante.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContratosController : ControllerBase
    {
        private readonly DataContext _context;

        public ContratosController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Contratos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContrato()
        {
            var mail = User.Identity.Name;
            return await _context.Contrato.Where(i => i.inmueble.Duenio.Email == mail).Include(i => i.inmueble)
                .Include(i => i.inquilino).ToListAsync();
        }

        // GET: api/Contratos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Contrato>>> GetContratoXInmueble(int id)
        {
            var contrato = await _context.Contrato.Include(i => i.inmueble)
                .Include(i => i.inquilino).Where(i => i.IdInmueble == id).ToListAsync();
            if (contrato == null)
            {
                return NotFound();
            }
            return contrato;
        }

        // PUT: api/Contratos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContrato(int id, Contrato contrato)
        {
            if (id != contrato.Id)
            {
                return BadRequest();
            }

            _context.Entry(contrato).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContratoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(contrato);
        }

        // POST: api/Contratos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contrato>> PostContrato(Contrato contrato)
        {
            try
            {
                _context.Contrato.Add(contrato);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetContrato", new { id = contrato.Id }, contrato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Contratos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContrato(int id)
        {
            var contrato = await _context.Contrato.FindAsync(id);
            if (contrato == null)
            {
                return NotFound();
            }

            _context.Contrato.Remove(contrato);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("inmuebles")]
        public async Task<ActionResult<IEnumerable<Inmueble>>> getInmueblesXContratos()
        {
            try
            {
                var mail = User.Identity.Name;
                return await _context.Contrato.Where(i => i.inmueble.Duenio.Email == mail).Include(i => i.inmueble)
                .Select(i => i.inmueble).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("contra")]
        public async Task<ActionResult<IEnumerable<Contrato>>> getContratosXInmueble(int id)
        {
            try
            {
                return await _context.Contrato.Include(i => i.inmueble).Where(i => i.IdInmueble == id).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("pagos/{id}")]
        public async Task<ActionResult<IEnumerable<Pago>>> getPagosXContrato(int id)
        {
            try
            {
                var pagos = _context.Pagos.Include(i => i.contrato).Where(i => i.IdContrato == id);
                return await _context.Pagos.Include(i => i.contrato).Where(i => i.IdContrato == id).ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private bool ContratoExists(int id)
        {
            return _context.Contrato.Any(e => e.Id == id);
        }
    }
}
