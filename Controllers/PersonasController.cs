using System.Collections;
using inmobiliariaGarroAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace inmobiliariaGarroAPI;

	[Route("api/[controller]")]
	[ApiController]
	public class PersonasController : ControllerBase  //
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public PersonasController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = env;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
             	return Ok(contexto.Personas.ToList());
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// GET: api/<controller>
		 [HttpGet("{id}")]
		public async Task<IActionResult> Get(int id)
		{
			try
			{
				
				var persona = contexto.Personas.FirstOrDefault(p => p.Id == id);
				if(persona == null) return NotFound();
             	return Ok(persona);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// POST: api/<controller>
		 [HttpPost]
		public async Task<IActionResult> Post([FromForm] Personas persona)
		{
			try
			{
				
				contexto.Personas.Add(persona);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("Get", new { id = persona.Id }, persona);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// PUT: api/<controller>
		 [HttpPut("{Id}")]
		public async Task<IActionResult> Post(int Id ,[FromForm] Personas persona)
		{
			try
			{
				var p = contexto.Personas.FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				p.DNI = persona.DNI;
				p.Nombre = persona.Nombre;
				p.Apellido = persona.Apellido;
				p.Telefono = persona.Telefono;
				contexto.Update(p);
				await contexto.SaveChangesAsync();
				return CreatedAtAction("Get", new { id = persona.Id }, p);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		// DELETE: api/<controller>
		 [HttpDelete("{Id}")]
		public async Task<IActionResult> Post(int Id)
		{
			try
			{
				var p = contexto.Personas.FirstOrDefault(p => p.Id == Id);
				if(p == null) return NotFound();
				contexto.Personas.Remove(p);
				await contexto.SaveChangesAsync();
				return NoContent();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
}