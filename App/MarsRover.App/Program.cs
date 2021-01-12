using MarsRover.Config.Models;
using MarsRover.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MarsRover.App
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = ConfigureServices();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                await serviceProvider.GetService<ImageDownloadApp>().RunAsync();
            }

        }
  
        private static IServiceCollection ConfigureServices()
        {
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{AppDomain.CurrentDomain.BaseDirectory}\\appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            services.AddLogging(config =>
            {
                config.AddSerilog(logger: logger, dispose: true);
            });

            var appConfig = new AppConfig();
            Configuration.GetSection("Input").Bind(appConfig);
            services.AddSingleton<IAppConfig>(appConfig);

            var marsRoverApiKeyConfig = new ApiKeyConfig();
            Configuration.GetSection("Auth:MarsRover").Bind(marsRoverApiKeyConfig);
            services.AddSingleton<IApiKeyConfig>(marsRoverApiKeyConfig);

            var marsRoverClientConfig = new HttpClientConfig();
            Configuration.GetSection("MarsRover").Bind(marsRoverClientConfig);

            services.AddHttpClient<IMarsRoverService, MarsRoverService>(
                client =>
                {
                    client.BaseAddress = marsRoverClientConfig.Url;
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

            services.AddHttpClient<IImageService, ImageService>();
            services.AddScoped<IFileService, FileService>();

            services.AddTransient<ImageDownloadApp>();

            return services;
        }
    }
}
