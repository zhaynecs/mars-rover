using System;
using MarsRover.Config.Models;
using MarsRover.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MarsRover.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var marsRoverClientConfig = new HttpClientConfig();
            Configuration.GetSection("MarsRover").Bind(marsRoverClientConfig);

            var marsRoverApiKeyConfig = new ApiKeyConfig();
            Configuration.GetSection("Auth:MarsRover").Bind(marsRoverApiKeyConfig);

            services.AddSingleton(marsRoverApiKeyConfig);

            services.AddHttpClient<IMarsRoverClient, MarsRoverClient>(
                client =>
                {
                    client.BaseAddress = marsRoverClientConfig.Url;
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });
            //.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));

            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IImageService, ImageService>();

            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
