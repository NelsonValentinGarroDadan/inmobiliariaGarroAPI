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
                return Ok(
                    contexto.Alquileres
                    .Where(i => i.Id == id)
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
		 [HttpGet("obtenerXPerfil")]
		public async Task<IActionResult> ObtenerXPerfil()
		{
			try
			{
                var usuario = User.Identity.Name;
                return Ok(
                    contexto.Alquileres
                    .Where(i => i.Inmueble.PropietarioId+"" == usuario)
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
    }