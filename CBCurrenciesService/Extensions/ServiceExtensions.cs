using CBCurrenciesService.Mapping;
using Contracts;
using Entities.Models;
using HttpService;
using HttpService.Helpers;
using LoggerService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace CBCurrenciesService.Extensions
{
	public static class ServiceExtensions
	{
		public static void ConfigureCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy("CorsPolicy",
					builder => builder.AllowAnyOrigin()
								.AllowAnyMethod()
								.AllowAnyHeader());
			});
		}

		public static void ConfigureIISIntegration(this IServiceCollection services)
		{
			services.Configure<IISOptions>(options =>
			{

			});
		}

		public static void ConfigureHttpService(this IServiceCollection services, IConfiguration config)
		{
			string httpString = config.GetSection("ConnectionStrings")["CbrCurrenciesHttpConnectionString"];
			var memoryCache = new MemoryCache(new MemoryCacheOptions());
			services.AddSingleton<IHttpCbrService>(s => new HttpCbrService(httpString, memoryCache, new DataHelper<SingleCurrencyData>()));
		}

		public static void ConfigureLoggerService(this IServiceCollection services)
		{
			services.AddSingleton<ILoggerManager, LoggerManager>();
		}

		public static void ConfigureMappingHelper(this IServiceCollection services)
		{
			services.AddScoped<IMappingHelper, MappingHelper>();
		}

	}
}
