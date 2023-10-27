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
	public class AlquileresController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public AlquileresController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
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
             	return Ok(
                    contexto.Alquileres
                    .Select(a => new
                    {
                        Id = a.Id,
                        Precio = a.Precio,
                        Fecha_Inicio = a.Fecha_Inicio,
                        Fecha_Fin  = a.Fecha_Fin ,
                        Inquilino = new {
                                        Id= a.Inquilino.Id ,
                                        Nombre = a.Inquilino.Persona.Nombre ,
                                        Apellido = a.Inquilino.Persona.Apellido
                                        },
                        Inmueble = new { 
                                        Id = a.Inmueble.Id,
                                        Direccion = a.Inmueble.Longitud+" "+a.Inmueble.Latitud,
                                        Propietario = new {
                                                        Id = a.Inmueble.Propietario.Id,
                                                        Nombre = a.Inmueble.Propietario.Persona.Nombre ,
                                                        Apellido = a.Inmueble.Propietario.Persona.Apellido
                                                        }
                                        }
                    })
                    .ToList()
                );
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
                var alquiler = contexto.Alquileres
                    .Where(i => i.Id == id)
                    .Include(a => a.Inquilino)
                        .ThenInclude(i => i.Persona)
                    .Include(a => a.Inmueble)
                        .ThenInclude(i => i.Propietario)
                        .ThenInclude(p => p.Persona)
                    .Select(a => new
                    {
                        Id = a.Id,
                        Precio = a.Precio,
                        Fecha_Inicio = a.Fecha_Inicio,
                        Fecha_Fin  = a.Fecha_Fin ,
                        Inquilino = new {
                                        Id= a.Inquilino.Id ,
                                        Nombre = a.Inquilino.Persona.Nombre ,
                                        Apellido = a.Inquilino.Persona.Apellido
                                        },
                        Inmueble = new { 
                                        Id = a.Inmueble.Id,
                                        Direccion = a.Inmueble.Longitud+" "+a.Inmueble.Latitud,
                                        Propietario = new {
                                                        Id = a.Inmueble.Propietario.Id,
                                                        Nombre = a.Inmueble.Propietario.Persona.Nombre ,
                                                        Apellido = a.Inmueble.Propietario.Persona.Apellido
                                                        }
                                        }
                    });

                if(alquiler == null) return NotFound();
                return Ok(alquiler);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        // GET: api/<controller>
		 [HttpGet("obtenerXInmueble")]
		public async Task<IActionResult> ObtenerXInmueble([FromQuery] int Id)
		{
			try
			{
                var alquiler = contexto.Alquileres
                    .Where(alquiler => alquiler.InmuebleId == Id)
                    .FirstOrDefault();
                return Ok(alquiler);
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
                var Alquileres = contexto.Alquileres
                .Include(a => a.Inmueble)
                .Include(a => a.Inquilino)
                    .ThenInclude(i => i.Persona)
                .Where(a => a.Inmueble.PropietarioId+"" ==usuario) 
                .ToList();
                 return Ok(Alquileres);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        
         //Alta
		// POST: api/<controller>
		 [HttpPost("create")]
		public async Task<IActionResult> Create([FromForm] decimal Precio ,
                                                [FromForm] DateTime Fecha_Inicio ,
                                                [FromForm] DateTime Fecha_Fin ,
                                                [FromForm] int InquilinoId ,
                                                [FromForm] int InmuebleId )
		{
			try
			{
				var alquiler = new Alquileres
				{
                    Precio = Precio ,
                    Fecha_Inicio = Fecha_Inicio ,
                    Fecha_Fin = Fecha_Fin ,
                    InquilinoId = InquilinoId ,
                    InmuebleId = InmuebleId
				};
				contexto.Alquileres.Add(alquiler);
				await contexto.SaveChangesAsync();
				var a = contexto.Alquileres
                    .Include(a => a.Inquilino)
                        .ThenInclude(i => i.Persona)
                    .Include(a => a.Inmueble)
                        .ThenInclude(i => i.Propietario)
                        .ThenInclude(p => p.Persona)
                    .Select(a => new
                    {
                        Id = a.Id,
                        Precio = a.Precio,
                        Fecha_Inicio = a.Fecha_Inicio,
                        Fecha_Fin  = a.Fecha_Fin ,
                        Inquilino = new {
                                        Id= a.Inquilino.Id ,
                                        Nombre = a.Inquilino.Persona.Nombre ,
                                        Apellido = a.Inquilino.Persona.Apellido
                                        },
                        Inmueble = new { 
                                        Id = a.Inmueble.Id,
                                        Direccion = a.Inmueble.Longitud+" "+a.Inmueble.Latitud,
                                        Propietario = new {
                                                        Id = a.Inmueble.Propietario.Id,
                                                        Nombre = a.Inmueble.Propietario.Persona.Nombre ,
                                                        Apellido = a.Inmueble.Propietario.Persona.Apellido
                                                        }
                                        }
                    })
					.FirstOrDefault(inm => inm.Id == alquiler.Id);
				if (a == null)
				{
					return NotFound();
				}
				return CreatedAtAction("ObtenerXId", new { id = a.Id },a);
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
				var a = contexto.Alquileres.FirstOrDefault(i => i.Id == Id);
				if(a == null) return NotFound();
				contexto.Alquileres.Remove(a);
				await contexto.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }