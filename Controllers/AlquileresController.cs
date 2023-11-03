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
        
        // POST: api/<controller>
		 [HttpPost("obtenerXInmueble")]
		public async Task<IActionResult> ObtenerXInmueble([FromBody] Inmuebles inmueble)
		{
			try
			{
                var alquiler = contexto.Alquileres
                    .Where(alquiler => alquiler.InmuebleId == inmueble.Id)
                    .Include(a => a.Inmueble)
                        .ThenInclude(i => i.Propietario)
                            .ThenInclude(p => p.Persona)
                    .Include(a => a.Inquilino)
                        .ThenInclude(i => i.Persona)
                    .FirstOrDefault();
                return Ok(alquiler);
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
       
    }