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
		
		//Obtener Logeado
		// GET: api/<controller>
		[HttpGet("perfil")]
		public async Task<IActionResult> Perfil()
		{
			try
			{
				var usuario = User.Identity.Name;
				var propietario = contexto.Propietarios.Include(p => p.Persona).FirstOrDefault(p => p.Id+"" == usuario);
				
             	return Ok(propietario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		//Update
		// PATCH: api/<controller>
		 [HttpPatch("update")]
		 
		public async Task<IActionResult> Update([FromForm] string Nombre,[FromForm] string Apellido,[FromForm] int DNI,[FromForm] long Telefono)
		{
			try
			{
				var usuario = User.Identity.Name;
				var p = contexto.Propietarios.Include(x => x.Persona).FirstOrDefault(p => p.Id+"" == usuario);
				if(p == null) return NotFound();
				p.Persona.Nombre =Nombre;
				p.Persona.Apellido = Apellido;
				p.Persona.Telefono = Telefono;
				p.Persona.DNI = DNI;
				contexto.Update(p);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("perfil", new { id = p.Id }, p);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return BadRequest(ex.Message);
			}
		}
		
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