using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InmobiliariaConstante.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace InmobiliariaConstante.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PropietariosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration config;

        public PropietariosController(DataContext context, IConfiguration config)
        {
            _context = context;
            this.config = config;
        }

        // GET: api/Propietarios
        [HttpGet("todos")]
        public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietarios()
        {
            return await _context.Propietarios.ToListAsync();
        }

        // GET: api/Propietarios/5
        [HttpGet]
        public async Task<ActionResult<Propietario>> GetPropietario ()
        {
            var usuario = User.Identity.Name;
            var propietario = await _context.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);

            if (propietario == null)
            {
                return NotFound();
            }

            return propietario;
        }

        // PUT: api/Propietarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut()]
        public async Task<IActionResult> PutPropietario([FromBody] Propietario propietario)
        {
            var usuario = User.Identity.Name;
            var id = await _context.Propietarios.AsNoTracking().SingleOrDefaultAsync(x => x.Email == usuario);
            if (id.IdPropietario != propietario.IdPropietario)
            {
                return BadRequest();
            }
            _context.Propietarios.Update(propietario);
            //_context.Entry(propietario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropietarioExists(id.IdPropietario))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(propietario);
        }

        // POST: api/Propietarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("crear")]
        public async Task<ActionResult<Propietario>> PostPropietario([FromForm] Propietario propietario)
        {
            try
            {
                _context.Propietarios.Add(propietario);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPropietario", new { id = propietario.IdPropietario }, propietario);
            }
            catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Propietarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePropietario(int id)
        {
            var propietario = await _context.Propietarios.FindAsync(id);
            if (propietario == null)
            {
                return NotFound();
            }

            _context.Propietarios.Remove(propietario);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PropietarioExists(int id)
        {
            return _context.Propietarios.Any(e => e.IdPropietario == id);
        }

        // POST api/<controller>/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginView loginView)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: loginView.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var p = await _context.Propietarios.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
                if (p == null || p.Clave != hashed)
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
                else
                {
                    var key = new SymmetricSecurityKey(
                        System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, p.Email),
                        new Claim("FullName", p.Nombre + " " + p.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: config["TokenAuthentication:Issuer"],
                        audience: config["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
