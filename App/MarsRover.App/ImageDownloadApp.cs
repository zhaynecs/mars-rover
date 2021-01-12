using MarsRover.Config.Models;
using MarsRover.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.App
{
    public class ImageDownloadApp
    {
        private readonly ILogger<ImageDownloadApp> logger;
        private readonly IAppConfig appConfig;
        private readonly IFileService fileService;
        private readonly IMarsRoverService marsRoverService;
        private readonly IImageService imageService;
            
        public ImageDownloadApp(ILogger<ImageDownloadApp> logger, IAppConfig appConfig, IFileService fileService, IMarsRoverService marsRoverService, IImageService imageService)
        {
            this.logger = logger;
            this.appConfig = appConfig;

            this.fileService = fileService;
            this.marsRoverService = marsRoverService;
            this.imageService = imageService;
        }

        public async Task RunAsync()
        {
            if (appConfig == null || string.IsNullOrEmpty(appConfig.Rover) || string.IsNullOrEmpty(appConfig.DatesUri))
            {
                this.logger.LogInformation($"Input data is incomplete or unavailable");
                throw new ArgumentNullException("appsettings.json > input");
            }

            if (!File.Exists(appConfig.DatesUri))
            {
                this.logger.LogInformation($"File {this.appConfig.DatesUri} does not exist");
                throw new ArgumentNullException(this.appConfig.DatesUri);
            }

            var length = new FileInfo(appConfig.DatesUri).Length;
            if (length == 0)
            {
                this.logger.LogInformation($"File {appConfig.DatesUri} is empty");
                throw new ArgumentException($"File {appConfig.DatesUri} is empty");
            }

            var dates = await this.fileService.ReadAsync(appConfig.DatesUri);
            this.logger.LogInformation($"Read {dates.Count()} valid dates");

            if (!Directory.Exists(this.appConfig.ImagesUri))
                Directory.CreateDirectory(this.appConfig.ImagesUri);

            var tasks = new List<Task>();
            foreach(var d in dates)
            {
                tasks.Add(Task.Run(async () =>
                {
                    this.logger.LogInformation($"Downloading image for {d.ToString("yyyy-MM-dd")}");

                    try
                    {
                        var data = await this.marsRoverService.GetImageDataAsync(this.appConfig.Rover, d);
                        if (data == null || data.Photos == null || !data.Photos.Any())
                        {
                            this.logger.LogError($"{this.appConfig.Rover} did not take any photos on {d.ToString("yyyy-MM-dd")}");
                            return;
                        }

                        var uri = data.Photos.First().Uri;
                        var path = Path.Combine($"{this.appConfig.ImagesUri}", $"{d.ToString("yyyy-MM-dd")}-{Path.GetFileName(uri)}");

                        var success = await this.imageService.DownloadImageAsync(uri, path);
                        if (success)
                            this.logger.LogInformation($"Successfully downloaded image for {d.ToString("yyyy-MM-dd")} to {path}");
                        else
                            this.logger.LogError($"Failed to download image for {d.ToString("yyyy-MM-dd")}");
                    }
                    catch(Exception e)
                    {
                        this.logger.LogError($"Failed to download image for {d.ToString("yyyy-MM-dd")}");
                        this.logger.LogError(e.Message);
                        this.logger.LogError(e.StackTrace);
                    }
                }));
            }

            await Task.WhenAll(tasks);

            this.logger.LogInformation("Done");
            Console.ReadLine();
        }
    }
}
