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
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext _context;

        public InmueblesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Inmuebles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inmueble>>> GetInmuebles()
        {
            return await _context.Inmuebles.Where(i => i.Duenio.Email ==User.Identity.Name).ToListAsync();
        }

        // GET: api/Inmuebles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inmueble>> GetInmueble(int id)
        {
            var inmueble = await _context.Inmuebles.FindAsync(id);

            if (inmueble == null)
            {
                return NotFound();
            }

            return inmueble;
        }

        // PUT: api/Inmuebles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutInmueble(int id, Inmueble inmueble)
        {
            if (id != inmueble.Id)
            {
                return BadRequest();
            }

            _context.Entry(inmueble).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InmuebleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Inmuebles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inmueble>> PostInmueble([FromForm]Inmueble inmueble)
        {
            try
            {
                inmueble.PropietarioId = _context.Propietarios.Single(e => e.Email == User.Identity.Name).IdPropietario;
                inmueble.Duenio = _context.Propietarios.Single(e => e.Email == User.Identity.Name);
                _context.Inmuebles.Add(inmueble);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetInmueble", new { id = inmueble.Id }, inmueble);
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> modificarEstado(int id)
        {
            var inmueble = await _context.Inmuebles.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
            if (inmueble == null)
            {
                return NotFound();
            }

            inmueble.Estado = !inmueble.Estado;
            _context.Inmuebles.Update(inmueble);
            await _context.SaveChangesAsync();

            return Ok(inmueble);
        }

        private bool InmuebleExists(int id)
        {
            return _context.Inmuebles.Any(e => e.Id == id);
        }
    }
}
