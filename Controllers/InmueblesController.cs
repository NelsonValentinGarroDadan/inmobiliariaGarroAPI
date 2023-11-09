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
		 [HttpGet("obtenerXPerfil")]
		public async Task<IActionResult> ObtenerXPerfil()
		{
			try
			{
				var usuario = User.Identity.Name;
				var usuarioId = Convert.ToInt32(usuario);
				var propiedades = contexto.Inmuebles
					.Where(i => i.PropietarioId == usuarioId)
					.ToList();
				return Ok(propiedades);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// GET: api/<controller>
		 [HttpGet("obtenerAlquiladosXPerfil")]
		public async Task<IActionResult> ObtenerAlquiladosXPerfil()
		{
			try
			{
                var usuario = User.Identity.Name;
				var usuarioId = Convert.ToInt32(usuario);
                var inmueblesAlquilados = contexto.Inmuebles
                .Join(contexto.Alquileres,
                    inmueble => inmueble.Id,
                    alquiler => alquiler.InmuebleId,
                    (inmueble, alquiler) => new { Inmueble = inmueble, Alquiler = alquiler })
                .Where(join => join.Inmueble.PropietarioId == usuarioId) 
                .Select(join => join.Inmueble) 
                .ToList();
                 return Ok(inmueblesAlquilados);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		 //Alta
		// POST: api/<controller>
		 [HttpPost("create")]
		public async Task<IActionResult> Create([FromForm] Inmuebles inmueble)
		{
			try
			{
				var u = User.Identity.Name;
				var usuarioId = Convert.ToInt32(u);
				var propietario = contexto.Propietarios.Include(prop=> prop.Persona).First(p => p.Id == usuarioId);
				inmueble.PropietarioId = propietario.Id;
				inmueble.Disponible = false;
				contexto.Inmuebles.Add(inmueble);

				contexto.SaveChanges();
				if(inmueble.ImagenFileName !=null && inmueble.Id>0){
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if(!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					string fileName = "imagen_" + inmueble.Id + Path.GetExtension(inmueble.ImagenFileName.FileName);
					string pathCompleto = Path.Combine(path, fileName);
					inmueble.Imagen = Path.Combine("/Uploads", fileName);
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						inmueble.ImagenFileName.CopyTo(stream);
					}
					contexto.Update(inmueble);
					contexto.SaveChanges();
				}
				
				return Ok(inmueble);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		// PATCH: api/<controller>
		 [HttpPatch("cambiarDisponibilidad")]
		 
		public async Task<IActionResult> CambiarDisponibilidad([FromBody] Inmuebles inmueble)
		{
			try
			{
				var usuarioId = Convert.ToInt32(User.Identity.Name);
				var i = contexto.Inmuebles
						.Where(inm => inm.Id == inmueble.Id && inm.Propietario.Id == usuarioId)
						.Include(i => i.Propietario)
							.ThenInclude(p => p.Persona)
						.FirstOrDefault();
				if(i == null) return NotFound();
				i.Disponible = inmueble.Disponible;

				contexto.Update(i);
				contexto.SaveChanges();
				return Ok(i);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		

	}