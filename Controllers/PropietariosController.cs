using System.Collections;
using System.Security.Claims;
using inmobiliariaGarroAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication;

namespace inmobiliariaGarroAPI;

	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
		public async Task<IActionResult> ObtenerTodos()
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
		public async Task<IActionResult> ObtenerXId(int id)
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
		public async Task<IActionResult> Create([FromForm] int PersonaId, [FromForm] string Mail, [FromForm] string Password)
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
		// DELETE: api/<controller>
		// POST: api/<controller>
		 [HttpDelete("delete/{Id}")]
		public async Task<IActionResult> Delete(int Id)
		{
			try
			{
				var p = contexto.Propietarios.FirstOrDefault(p => p.Id == Id);
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
		//Obtener Logeado
		// GET: api/<controller>
		[HttpGet("perfil")]
		public async Task<IActionResult> Perfil()
		{
			try
			{
				var usuario = User.Identity.Name;
				var propietario = contexto.Propietarios.Include(p => p.Persona).FirstOrDefault(p => p.Id+"" == usuario);
				propietario.Id = 0;
				propietario.Persona.Id=0;
             	return Ok(propietario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Update
		// PUT: api/<controller>
		 [HttpPut("update")]
		 
		public async Task<IActionResult> Update([FromForm] Propietarios propietario)
		{
			try
			{
				var usuario = User.Identity.Name;
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Id+"" == usuario);
				if(p == null) return NotFound();
				p.Mail = propietario.Mail;
				p.Persona.Nombre = propietario.Persona.Nombre;
				p.Persona.Apellido = propietario.Persona.Apellido;
				p.Persona.Telefono = propietario.Persona.Telefono;
				p.Persona.DNI = propietario.Persona.DNI;
				contexto.Update(p);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("perfil", new { id = p.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Cambio de Contraseña
		// PUT: api/<controller>
		 [HttpPut("cambiarContraseña")]
		public async Task<IActionResult> CambiarContraseña([FromForm] string nuevaClave )
		{
			try
			{
				if(nuevaClave == "") return BadRequest("La clave no puede ser vacia");
				var usuario = User.Identity.Name;
				var propietario = contexto.Propietarios.Include(p => p.Persona).FirstOrDefault(p => p.Id+"" == usuario);		
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Id == propietario.Id);
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
				return CreatedAtAction("perfil", new { id = p.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		// POST: api/<controller>
		//Login
		[AllowAnonymous]
		 [HttpPost("login")]
		public async Task<IActionResult> Login([FromForm] string Mail,[FromForm] string Password)
		{
			try
			{
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Mail == Mail);
				if(p == null) return  BadRequest("Usuario o Contraseña incorrecta");
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: Password,
                        salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 250 / 8
                    ));
				if(p.Password != hashed) return  BadRequest("Usuario o Contraseña incorrecta");
				var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
				var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, p.Id+""),
					new Claim(ClaimTypes.Role, "Propietario"),
				};

				var token = new JwtSecurityToken(
					issuer: config["TokenAuthentication:Issuer"],
					audience: config["TokenAuthentication:Audience"],
					claims: claims,
					expires: DateTime.Now.AddMinutes(60),
					signingCredentials: credenciales
				);
				
				//retornar el JWT
				return Ok(new JwtSecurityTokenHandler().WriteToken(token));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
}