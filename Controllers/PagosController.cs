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
	public class PagosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public PagosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
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
                    contexto.Pagos
                    .Include(p => p.Alquiler)
                        .ThenInclude(a => a.Inquilino)
                    .Include(a => a.Alquiler)
                        .ThenInclude(a => a.Inmueble)
                            .ThenInclude(inm => inm.Propietario)
                                .ThenInclude(prop => prop.Persona)
                    .Select(p => new
                    {
                        Id = p.Id,
                        NroPAgo = p.NroPago,
                        Alquiler = new {
                            Id = p.Alquiler.Id,
                            Inquilino = new {
                                        Id= p.Alquiler.Inquilino.Id ,
                                        Nombre = p.Alquiler.Inquilino.Persona.Nombre ,
                                        Apellido = p.Alquiler.Inquilino.Persona.Apellido
                                        },
                            Inmueble = new { 
                                        Id = p.Alquiler.Inmueble.Id,
                                        Direccion =p.Alquiler.Inmueble.Longitud+" "+p.Alquiler.Inmueble.Latitud,
                                        Propietario = new {
                                                        Id = p.Alquiler.Inmueble.Propietario.Id,
                                                        Nombre = p.Alquiler.Inmueble.Propietario.Persona.Nombre ,
                                                        Apellido = p.Alquiler.Inmueble.Propietario.Persona.Apellido
                                                        }
                                        }
                        },
                        Fecha  = p.Fecha ,
                        Importe = p.Importe

                        
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
                var pago = contexto.Pagos
                    .Where(i => i.Id == id)
                    .Include(p => p.Alquiler)
                        .ThenInclude(a => a.Inquilino)
                    .Include(a => a.Alquiler)
                        .ThenInclude(a => a.Inmueble)
                            .ThenInclude(inm => inm.Propietario)
                                .ThenInclude(prop => prop.Persona)
                    .Select(p => new
                    {
                        Id = p.Id,
                        NroPAgo = p.NroPago,
                        Alquiler = new {
                            Id = p.Alquiler.Id,
                            Inquilino = new {
                                        Id= p.Alquiler.Inquilino.Id ,
                                        Nombre = p.Alquiler.Inquilino.Persona.Nombre ,
                                        Apellido = p.Alquiler.Inquilino.Persona.Apellido
                                        },
                            Inmueble = new { 
                                        Id = p.Alquiler.Inmueble.Id,
                                        Direccion =p.Alquiler.Inmueble.Longitud+" "+p.Alquiler.Inmueble.Latitud,
                                        Propietario = new {
                                                        Id = p.Alquiler.Inmueble.Propietario.Id,
                                                        Nombre = p.Alquiler.Inmueble.Propietario.Persona.Nombre ,
                                                        Apellido = p.Alquiler.Inmueble.Propietario.Persona.Apellido
                                                        }
                                        }
                        },
                        Fecha  = p.Fecha ,
                        Importe = p.Importe

                        
                    });
                if(pago==null ) return NotFound();
                return Ok(pago);
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
                    contexto.Pagos
                    .Include(p => p.Alquiler)
                        .ThenInclude(a => a.Inquilino)
                    .Include(a => a.Alquiler)
                        .ThenInclude(a => a.Inmueble)
                            .ThenInclude(inm => inm.Propietario)
                                .ThenInclude(prop => prop.Persona)
                    
                    .Where(i => i.Alquiler.Inmueble.PropietarioId+"" == usuario)
                    .Select(p => new
                    {
                        Id = p.Id,
                        NroPAgo = p.NroPago,
                        Alquiler = new {
                            Id = p.Alquiler.Id,
                            Inquilino = new {
                                        Id= p.Alquiler.Inquilino.Id ,
                                        Nombre = p.Alquiler.Inquilino.Persona.Nombre ,
                                        Apellido = p.Alquiler.Inquilino.Persona.Apellido
                                        },
                            Inmueble = new { 
                                        Id = p.Alquiler.Inmueble.Id,
                                        Direccion =p.Alquiler.Inmueble.Longitud+" "+p.Alquiler.Inmueble.Latitud,
                                        Propietario = new {
                                                        Id = p.Alquiler.Inmueble.Propietario.Id,
                                                        Nombre = p.Alquiler.Inmueble.Propietario.Persona.Nombre ,
                                                        Apellido = p.Alquiler.Inmueble.Propietario.Persona.Apellido
                                                        }
                                        }
                        },
                        Fecha  = p.Fecha ,
                        Importe = p.Importe

                        
                    }).ToList()
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
		public async Task<IActionResult> Create([FromForm] int NroPago ,
                                                [FromForm] int AlquilerId,
                                                [FromForm] DateTime Fecha,
                                                [FromForm] decimal Importe )
		{
			try
			{
				var pago = new Pagos
				{
                    NroPago = NroPago,
                    AlquilerId = AlquilerId,
                    Fecha = Fecha,
                    Importe = Importe 
				};
				contexto.Pagos.Add(pago);
				await contexto.SaveChangesAsync();
				var p = contexto.Pagos
                    .Include(p => p.Alquiler)
                        .ThenInclude(a => a.Inquilino)
                    .Include(a => a.Alquiler)
                        .ThenInclude(a => a.Inmueble)
                            .ThenInclude(inm => inm.Propietario)
                                .ThenInclude(prop => prop.Persona)
                    .Select(p => new
                    {
                        Id = p.Id,
                        NroPAgo = p.NroPago,
                        Alquiler = new {
                            Id = p.Alquiler.Id,
                            Inquilino = new {
                                        Id= p.Alquiler.Inquilino.Id ,
                                        Nombre = p.Alquiler.Inquilino.Persona.Nombre ,
                                        Apellido = p.Alquiler.Inquilino.Persona.Apellido
                                        },
                            Inmueble = new { 
                                        Id = p.Alquiler.Inmueble.Id,
                                        Direccion =p.Alquiler.Inmueble.Longitud+" "+p.Alquiler.Inmueble.Latitud,
                                        Propietario = new {
                                                        Id = p.Alquiler.Inmueble.Propietario.Id,
                                                        Nombre = p.Alquiler.Inmueble.Propietario.Persona.Nombre ,
                                                        Apellido = p.Alquiler.Inmueble.Propietario.Persona.Apellido
                                                        }
                                        }
                        },
                        Fecha  = p.Fecha ,
                        Importe = p.Importe

                        
                    })
					.FirstOrDefault(p => p.Id == pago.Id);
				if (p == null)
				{
					return NotFound();
				}
				return CreatedAtAction("ObtenerXId", new { id = p.Id },p);
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
				var p = contexto.Pagos.FirstOrDefault(i => i.Id == Id);
				if(p == null) return NotFound();
				contexto.Pagos.Remove(p);
				await contexto.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }