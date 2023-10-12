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
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class InmueblesController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public InmueblesController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
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
             	return Ok(contexto.Inmuebles
				.Select(i => new
				{
					Id = i.Id,
					Longitud = i.Longitud,
					Latitud = i.Latitud,
					CantidadAmbientes = i.CAmbientes,
					Tipo = i.Tipo,
					Uso = i.Uso,
					Disponible = i.Disponible,
					Propietario = new
					{
						Id = i.Propietario.Id,
						Nombre = i.Propietario.Persona.Nombre,
						Apellido = i.Propietario.Persona.Apellido
					} 
				})
				.ToList());
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
				Console.WriteLine("Hola");
				var inmueble = contexto.Inmuebles
					.Where(i => i.Id == id)
					.Select(i => new
					{
						Longitud = i.Longitud,
						Latitud = i.Latitud,
						CantidadAmbientes = i.CAmbientes,
						Tipo = i.Tipo,
						Uso = i.Uso,
						Disponible = i.Disponible,
						Propietario = new
						{
							Id = i.Propietario.Id,
							Nombre = i.Propietario.Persona.Nombre,
							Apellido = i.Propietario.Persona.Apellido
						} 
					})
					.FirstOrDefault();

				if (inmueble == null) return NotFound();
        		return Ok(new
					{
						Longitud = inmueble.Longitud,
						Latitud = inmueble.Latitud,
						CantidadAmbientes = inmueble.CAmbientes,
						Tipo = inmueble.Tipo,
						Uso = inmueble.Uso,
						Disponible = inmueble.Disponible,
						Propietario = new
						{
							Id = inmueble.Propietario.Id,
							Nombre = inmueble.Propietario.Persona.Nombre,
							Apellido = inmueble.Propietario.Persona.Apellido
						}
					}
				);
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		 // GET: api/<controller>
		 [HttpGet("obtenerXPerfil")]
		public async Task<IActionResult> obtenerXPerfil()
		{
			try
			{
				var usuario = User.Identity.Name;
				var propietario = contexto.Propietarios.Include(p => p.Persona).FirstOrDefault(p => p.Id+"" == usuario);
				return Ok(
					contexto.Inmuebles
					.Where(i => i.PropietarioId == propietario.Id)
					.Select(i => new
					{
						Longitud = i.Longitud,
						Latitud = i.Latitud,
						CantidadAmbientes = i.CAmbientes,
						Tipo = i.Tipo,
						Uso = i.Uso,
						Disponible = i.Disponible
					})
					.ToList()
				);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		 //Alta
		// POST: api/<controller>
		 [HttpPost("create")]
		public async Task<IActionResult> Create([FromForm] string Longitud , 
												[FromForm] string Latitud ,
												[FromForm] int CAmbientes ,
												[FromForm] string Tipo ,
												[FromForm] string Uso ,
												[FromForm] decimal Precio ,
												[FromForm] bool Disponible ,
												[FromForm] int PropietarioId )
		{
			try
			{
				var inmueble = new Inmuebles
				{
					Longitud = Longitud, 
					Latitud = Latitud,
					CAmbientes = CAmbientes,
					Tipo = Tipo,
					Uso = Uso,
					Precio = Precio,
					Disponible = Disponible,
					PropietarioId = PropietarioId
				};
				contexto.Inmuebles.Add(inmueble);
				await contexto.SaveChangesAsync();
				var i = contexto.Inmuebles
					.Include(inm => inm.Propietario)
						.ThenInclude(p => p.Persona)
					.FirstOrDefault(inm => inm.Id == inmueble.Id);
				if (i == null)
				{
					return NotFound();
				}
				return CreatedAtAction("ObtenerXId", new { id = i.Id },i);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }