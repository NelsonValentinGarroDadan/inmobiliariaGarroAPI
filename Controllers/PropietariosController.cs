using System.Collections;
using System.Security.Claims;
using inmobiliariaGarroAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace inmobiliariaGarroAPI;

	[Route("api/[controller]")]
	[ApiController]
	public class PropietariosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public PropietariosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = env;
		}
		// GET: api/<controller>
		[HttpGet("obtenerTodos")]
		public async Task<IActionResult> Get()
		{
			try
			{
             	return Ok(contexto.Propietarios.Include(p => p.Persona).ToList());
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// GET: api/<controller>
		 [HttpGet("obtenerXId/{id}")]
		public async Task<IActionResult> obtenerXId(int id)
		{
			try
			{
				
				var propietario = contexto.Propietarios.Include(p => p.Persona).FirstOrDefault(p => p.Id == id);
				if(propietario == null) return NotFound();
             	return Ok(propietario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Alta
		// POST: api/<controller>
		 [HttpPost("create")]
		public async Task<IActionResult> Post([FromForm] int PersonaId, [FromForm] string Mail, [FromForm] string Password)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 250 / 8
                    ));
				var propietario = new Propietarios
				{
					PersonaId = PersonaId,
					Mail = Mail,
					Password = hashed
				};
				contexto.Propietarios.Add(propietario);
				await contexto.SaveChangesAsync();
				var p = contexto.Propietarios
					.Include(prop => prop.Persona)
					.FirstOrDefault(p => p.Id == propietario.Id);

				if (p == null)
				{
					return NotFound();
				}
				return CreatedAtAction("obtenerXId", new { id = p.Id },p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Update
		// PUT: api/<controller>
		 [HttpPut("update/{Id}")]
		public async Task<IActionResult> update(int Id ,[FromForm] string Mail)
		{
			try
			{
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				p.Mail = Mail;
				contexto.Update(p);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("obtenerXId", new { id = p.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Cambio de Contraseña
		// PUT: api/<controller>
		 [HttpPut("cambiarContraseña/{Id}")]
		public async Task<IActionResult> cambiarContraseña(int Id ,[FromForm] string nuevaClave )
		{
			try
			{
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: nuevaClave,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 250 / 8
                    ));
				p.Password = hashed;
				contexto.Update(p);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("obtenerXId", new { id = p.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// DELETE: api/<controller>
		// POST: api/<controller>
		 [HttpDelete("delete/{Id}")]
		public async Task<IActionResult> Post(int Id)
		{
			try
			{
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				contexto.Propietarios.Remove(p);
				await contexto.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// POST: api/<controller>
		//Login
		 [HttpPost("login")]
		public async Task<IActionResult> Login([FromForm] string Mail,[FromForm] string Password)
		{
			try
			{
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Mail == Mail);
				if(p == null) return NotFound();
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 250 / 8
                    ));
				if(p.Password != hashed) return NotFound();

				var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, p.Id+""),
                };
                    
                var claimIdentity = new ClaimsIdentity(
                    claims,CookieAuthenticationDefaults.AuthenticationScheme
                );
                    
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimIdentity)
                );
				//retornar el JWT
				return Ok("Logueado");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// POST: api/<controller>
		//Logout
		[HttpPost("logout")]
		public async Task<IActionResult> Logout()
		{
			try
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
}