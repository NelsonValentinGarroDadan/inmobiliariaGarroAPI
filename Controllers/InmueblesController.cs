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
		 [HttpGet("obtenerXId/{id}")]
		public async Task<IActionResult> ObtenerXId(int id)
		{
			try
			{
				var inmueble = contexto.Inmuebles
					.Where(i => i.Id == id)
					.Select(i => new
					{
						Id = i.Id,
						Longitud = i.Longitud,
						Latitud = i.Latitud,
						CantidadAmbientes = i.CAmbientes,
						Tipo = i.Tipo,
						Uso = i.Uso,
						Disponible = i.Disponible,
						Precio = i.Precio,
						Propietario = new
						{
							Id = i.Propietario.Id,
							Nombre = i.Propietario.Persona.Nombre,
							Apellido = i.Propietario.Persona.Apellido
						} 
					});

				if (inmueble == null) return NotFound();
        		return Ok(inmueble);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
					.Select(i => new {
						Id = i.Id,
						Longitud = i.Longitud,
						Latitud = i.Latitud,
						CAmbientes = i.CAmbientes,
						Tipo = i.Tipo,
						Uso = i.Uso,
						Disponible = i.Disponible,
						Imagen = i.Imagen,
						Precio = i.Precio,
						PropietarioId = i.PropietarioId

					})
					.Where(i => i.PropietarioId == usuarioId)
					.ToList();
				if(propiedades == null) return NotFound();
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
				var propietario = await contexto.Propietarios.FirstAsync(p => p.Id+"" == u);
				inmueble.PropietarioId = propietario.Id;
				inmueble.Disponible = false;
				contexto.Inmuebles.Add(inmueble);

				await contexto.SaveChangesAsync();
				if(inmueble.ImagenFileName !=null && inmueble.Id>0){
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if(!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}

					string fileName = "imagen_" + inmueble.Id + Path.GetExtension(inmueble.ImagenFileName.FileName);
					string pathCompleto = Path.Combine(path, fileName);
					inmueble.Imagen = fileName;
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						inmueble.ImagenFileName.CopyTo(stream);
					}
					contexto.Update(inmueble);
					await contexto.SaveChangesAsync();
				}
				
				return CreatedAtAction("ObtenerXId", new { id = inmueble.Id },inmueble);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		// PATCH: api/<controller>
		 [HttpPatch("cambiarDisponibilidad")]
		 
		public async Task<IActionResult> CambiarDisponibilidad([FromForm] Inmuebles inmueble)
		{
			try
			{
				var usuarioId = Convert.ToInt32(User.Identity.Name);
				var i = contexto.Inmuebles.FirstOrDefault(inm => inm.Id == inmueble.Id && inm.Propietario.Id == inmueble.Propietario.Id);
				if(i == null) return NotFound();
				i.Disponible = inmueble.Disponible;

				contexto.Update(i);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("obtenerXId", new { id = i.Id }, i);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		
		// GET: api/<controller>
		 
		[AllowAnonymous]
		[HttpGet("Imagenes/{nombreImagen}")]
		public IActionResult ObtenerImagen(string nombreImagen)
		{
			try
			{
				// Asegúrate de que nombreImagen sea seguro y válido.
				// Por ejemplo, verifica que no contenga rutas relativas.

				string wwwRootPath = environment.WebRootPath; // Obten la ruta raíz del servidor web
				string filePath = Path.Combine(wwwRootPath, "Uploads", nombreImagen);

				var imageBytes = System.IO.File.ReadAllBytes(filePath);

				return File(imageBytes, "image/jpeg"); // Cambia "image/jpeg" al tipo MIME correcto de tu imagen
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}