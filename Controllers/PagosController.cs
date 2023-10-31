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
		 [HttpGet("obtenerXAlquiler")]
		public async Task<IActionResult> ObtenerXInmueble([FromQuery] Alquileres alquiler)
		{
			try
			{
                var pago = contexto.Pagos
                    .Where(i => i.AlquilerId == alquiler.Id)
                    .Include(p => p.Alquiler)
                        .ThenInclude(a => a.Inquilino)
                    .Include(a => a.Alquiler)
                        .ThenInclude(a => a.Inmueble)
                            .ThenInclude(inm => inm.Propietario)
                                .ThenInclude(prop => prop.Persona)
                    .Select(p => new
                    {
                        Id = p.Id,
                        NroPago = p.NroPago,
                        Alquiler = new {
                            Id = p.Alquiler.Id,
                            Inquilino = new {
                                        Id= p.Alquiler.Inquilino.Id ,
                                        Nombre = p.Alquiler.Inquilino.Persona.Nombre ,
                                        Apellido = p.Alquiler.Inquilino.Persona.Apellido
                                        },
                            Inmueble = new { 
                                        Id = p.Alquiler.Inmueble.Id,
                                        Longitud =p.Alquiler.Inmueble.Longitud,
                                        Latitud =p.Alquiler.Inmueble.Latitud
                                        }
                        },
                        Fecha  = p.Fecha ,
                        Importe = p.Importe

                        
                    }).ToList();
                if(pago==null ) return NotFound();
                return Ok(pago);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        
    }