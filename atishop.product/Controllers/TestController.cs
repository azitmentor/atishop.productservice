using Atishop.Dataprovider.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atishop.product.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TestController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing1", "Scorching"
		};

		private readonly ILogger<TestController> _logger;
		private readonly MainDbContext _dbContext;

		public TestController(ILogger<TestController> logger, MainDbContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		[HttpGet]
		public IEnumerable<WeatherForecast> Get()
		{
			var rng = new Random();
			return Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateTime.Now.AddDays(index),
				TemperatureC = rng.Next(-20, 55),
				Summary = Summaries[rng.Next(Summaries.Length)]
			})
			.ToArray();
		}

		[HttpGet("product")]
		public IEnumerable<t_product> Product()
		{
			return _dbContext.t_product.ToList();
		}

		[HttpGet("insert")]
		public string Insert()
		{
			for (int i = 0; i < 500; i++)
			{
				_dbContext.t_product.Add(new t_product() { eancode = "89123123", ProductName = DateTime.Now.ToString() });
			}
			_dbContext.SaveChanges();
			return "ok";
		}
	}
}
