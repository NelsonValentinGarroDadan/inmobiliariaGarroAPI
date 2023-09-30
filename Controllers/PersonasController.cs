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
			environment = env;
		}
		// GET: api/<controller>
		[HttpGet]
		public IEnumerable<Personas> Get()
		{
			try
			{
                return contexto.Personas.ToList();
			}
			catch (Exception ex)
			{
                var list = new List<Personas>();
                list.Add(new Personas());
				return list;
			}
		}
}