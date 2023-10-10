using System.Collections;
using inmobiliariaGarroAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace inmobiliariaGarroAPI;

	[Route("api/[controller]")]
	[ApiController]
	public class PropietarioController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public PersonasController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = env;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
             	return Ok(contexto.Propietarios.ToList());
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// GET: api/<controller>
		 [HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				
				var propietario = contexto.Propietarios.FirstOrDefault(p => p.Id == id);
				if(persona == null) return NotFound();
             	return Ok(persona);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Alta
		// POST: api/<controller>
		 [HttpPost]
		public async Task<IActionResult> Post([FromForm] Propietarios propietario)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 250 / 8
                    ));
				propietario.Password = hashed;
				contexto.Propietarios.Add(propietario);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("Get", new { id = propietario.Id }, propietario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Update
		// PUT: api/<controller>
		 [HttpPut("{Id}")]
		public async Task<IActionResult> Post(int Id ,[FromForm] Propietarios propietario)
		{
			try
			{
				var p = contexto.Propietarios.FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				p.PersonaId = propietario.PersonaId;
				p.Mail = propietario.Mail;
				contexto.Update(p);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("Get", new { id = persona.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Cambio de Contraseña
		// PUT: api/<controller>
		 [HttpPut("{Id}")]
		public async Task<IActionResult> Post(int Id ,[FromForm] string nuevaClave )
		{
			try
			{
				var p = contexto.Propietarios.FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: nuevaClave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 250 / 8
                    ));
				p.Password = hashed;
				contexto.Update(p);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("Get", new { id = persona.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// DELETE: api/<controller>
		// POST: api/<controller>
		 [HttpDelete("{Id}")]
		public async Task<IActionResult> Post(int Id)
		{
			try
			{
				var p = contexto.Propietario.FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				contexto.Personas.Remove(p);
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
		 [HttpPost]
		public async Task<IActionResult> Login([FromForm] string Mail,[FromForm] string Password)
		{
			try
			{
				var p = contexto.Propietario.FirstOrDefault(p => p.Mail == Mail);
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

				return CreatedAtAction("Get", new { id = p.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// POST: api/<controller>
		//Logout
		[HttpPost]
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