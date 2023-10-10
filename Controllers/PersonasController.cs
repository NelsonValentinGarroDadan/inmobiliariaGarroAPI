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
		public IActionResult Get()
		{
             return Ok(contexto.Personas.ToList());
		}
}