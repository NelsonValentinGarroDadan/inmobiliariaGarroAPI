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
					Precio = i.Precio,
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
					.Where(i => i.PropietarioId+"" == usuario)
					.ToList();
				if(propiedades == null) return NotFound();
				return Ok(propiedades);
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
												[FromForm] int PropietarioId ,
												[FromForm] IFormFile imagen)
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
					Disponible = false,
					PropietarioId = PropietarioId,
					ImagenFileName = imagen,
					Imagen = ""
				};
				contexto.Inmuebles.Add(inmueble);
				
				await contexto.SaveChangesAsync();
				var i = contexto.Inmuebles
					.FirstOrDefault(inm => inm.Id == inmueble.Id);
				string wwwPath = environment.WebRootPath;
                string path = Path.Combine(wwwPath, "Uploads");
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = "imagen_" + i.Id + Path.GetExtension(inmueble.ImagenFileName.FileName);
                string pathCompleto = Path.Combine(path, fileName);
                i.Imagen = fileName;
                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    inmueble.ImagenFileName.CopyTo(stream);
                }
				contexto.Update(i);
				await contexto.SaveChangesAsync();
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
		// PUT: api/<controller>
		 [HttpPut("update")]
		 
		public async Task<IActionResult> Update([FromForm] int Id,
												[FromForm] string Longitud , 
												[FromForm] string Latitud ,
												[FromForm] int CAmbientes ,
												[FromForm] string Tipo ,
												[FromForm] string Uso ,
												[FromForm] decimal Precio )
		{
			try
			{
				var i = contexto.Inmuebles.FirstOrDefault(i => i.Id == Id);
				if(i == null) return NotFound();

				i.Longitud = Longitud;
				i.Latitud = Latitud;
				i.CAmbientes = CAmbientes;
				i.Tipo = Tipo;
				i.Uso = Uso;
				i.Precio = Precio;
				

				contexto.Update(i);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("obtenerXId", new { id = i.Id }, i);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// PATCH: api/<controller>
		 [HttpPatch("cambiarDisponibilidad")]
		 
		public async Task<IActionResult> CambiarDisponibilidad([FromForm] int Id,[FromForm] bool estado)
		{
			try
			{
				var i = contexto.Inmuebles.FirstOrDefault(inm => inm.Id == Id);
				if(i == null) return NotFound();
				i.Disponible = estado;

				contexto.Update(i);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("obtenerXId", new { id = i.Id }, i);
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
				var i = contexto.Inmuebles.FirstOrDefault(inm => inm.Id == Id);
				if(i == null) return NotFound();
				contexto.Inmuebles.Remove(i);
				await contexto.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
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