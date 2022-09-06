using atipshop.apigateway;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace atishop.apigateway.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GatewayController : ControllerBase
	{
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<GatewayController> _logger;
		private readonly IHttpClientFactory _httpClientFactory;

		public GatewayController(ILogger<GatewayController> logger, IHttpClientFactory httpClientFactory)
		{
			_logger = logger;
			_httpClientFactory = httpClientFactory;
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
		public string GetProduct()
		{
			try
			{
				var client = _httpClientFactory.CreateClient();
				return client.GetStringAsync("http://atishopproduct-service/test/product").Result;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}

		[HttpGet("grpc")]
		public async Task<string> GetGRPCAsync()
		{
			// The port number must match the port of the gRPC server.
			using var channel = GrpcChannel.ForAddress("http://atishopcustomer-service", new GrpcChannelOptions() { Credentials = Grpc.Core.ChannelCredentials.Insecure });
			var client = new Greeter.GreeterClient(channel);
			var reply = await client.SayHelloAsync(
							  new HelloRequest { Name = "GreeterClient" });
			return reply.Message;
		}

		[HttpGet("grpc2")]
		public async Task<string> GetGRPC2Async()
		{
			var httpHandler = new HttpClientHandler();
			// Return `true` to allow certificates that are untrusted/invalid
			httpHandler.ServerCertificateCustomValidationCallback =
				HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

			// The port number must match the port of the gRPC server.
			using var channel = GrpcChannel.ForAddress("https://atishopcustomer-service", new GrpcChannelOptions() { HttpHandler = httpHandler });
			var client = new Greeter.GreeterClient(channel);
			var reply = await client.SayHelloAsync(
							  new HelloRequest { Name = "GreeterClient" });
			return reply.Message;
		}
	}
}
