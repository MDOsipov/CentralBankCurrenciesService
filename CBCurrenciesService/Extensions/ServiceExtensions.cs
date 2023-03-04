using CBCurrenciesService.Mapping;
using Contracts;
using HttpService;
using LoggerService;
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
			services.AddSingleton<IHttpCbrService>(s => new HttpCbrService(httpString));
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
