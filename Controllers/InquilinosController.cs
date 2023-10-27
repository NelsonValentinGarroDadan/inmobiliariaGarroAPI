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

        // GET: api/<controller>
		[HttpGet("obtenerTodos")]
		public async Task<IActionResult> ObtenerTodos()
		{
			try
			{
             	return Ok(contexto.Inquilinos.Include(p => p.Persona).ToList());
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
				
				var inquilino = contexto.Inquilinos.Include(i => i.Persona).FirstOrDefault(i => i.Id == id);
				if(inquilino == null) return NotFound();
             	return Ok(inquilino);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
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
        //Alta
		// POST: api/<controller>
		 [HttpPost("create")]
		public async Task<IActionResult> Create([FromForm] int PersonaId, [FromForm] string Longitud, [FromForm] string Latitud)
		{
			try
			{
				var inquilino = new Inquilinos
				{
					PersonaId = PersonaId,
					Longitud = Longitud,
					Latitud = Latitud
				};
				contexto.Inquilinos.Add(inquilino);
				await contexto.SaveChangesAsync();
				var p = contexto.Inquilinos
					.Include(prop => prop.Persona)
					.FirstOrDefault(p => p.Id == inquilino.Id);
				if (p == null)
				{
					return NotFound();
				}
				return CreatedAtAction("obtenerXId", new { id = p.Id },p);
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
				var i = contexto.Inquilinos.Include(i => i.Persona).FirstOrDefault(i => i.Id == Id);
				if(i == null) return NotFound();
				contexto.Inquilinos.Remove(i);
				await contexto.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        //Update
		// PUT: api/<controller>
		 [HttpPut("update/{id}")]
		 
		public async Task<IActionResult> update(int Id, [FromForm] Inquilinos inquilino)
		{
			try
			{
				var i = contexto.Inquilinos.Include(x => x.Persona).FirstOrDefault(i => i.Id == Id);
				if(i == null) return NotFound();
				i.Persona.Nombre = inquilino.Persona.Nombre;
				i.Persona.Apellido = inquilino.Persona.Apellido;
				i.Persona.Telefono = inquilino.Persona.Telefono;
				i.Persona.DNI = inquilino.Persona.DNI;
				contexto.Update(i);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("obtenerXId", new { id = i.Id }, i);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
    }