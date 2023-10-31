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
	public class InquilinosController : ControllerBase
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public InquilinosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = env;
		}
        
		 //GET: api/<controller>
        [HttpGet("obtenerXInmueble")]
        public async Task<IActionResult> ObtenerXInmueble([FromQuery] int Id)
		{
			try
			{
                var alquiler = contexto.Alquileres.FirstOrDefault(a => a.InmuebleId == Id);

				if (alquiler != null)
				{
					int inquilinoId = alquiler.InquilinoId;

					var inquilino = contexto.Inquilinos.Include(p=> p.Persona).FirstOrDefault(i => i.Id == inquilinoId);

					if (inquilino != null)
    				{
                		return Ok(inquilino);
					}else{
						return NotFound();
					}
				}else{
						return NotFound();
				}
            }
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
        }
        
    }