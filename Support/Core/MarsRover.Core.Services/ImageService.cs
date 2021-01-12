using MarsRover.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public class ImageService: IImageService
    {
        private readonly ILogger<ImageService> logger;

        private readonly IMarsRoverService marsRoverService;
        private readonly HttpClient httpClient;

        public ImageService(ILogger<ImageService> logger, IMarsRoverService marsRoverService, HttpClient httpClient)
        {
            this.logger = logger;
            this.marsRoverService = marsRoverService;
            this.httpClient = httpClient;
        }

        public async Task<bool> DownloadMarsRoverImagesAsync(string rover, IEnumerable<DateTime> dates)
        {
            if (dates == null)
                throw new ArgumentException("No dates provided");

            foreach (var d in dates)
            {
                try
                {
                    await foreach (var r in this.marsRoverService.GetImagesAsync(rover, d))
                    {
                        if (r == null || r.Photos == null || !r.Photos.Any())
                        {
                            this.logger.LogError($"No images were found on {d.Date}");
                            continue;
                        }

                        var photoUri = r.Photos.ElementAt(r.Photos.Count() / 2).Uri;
                        var fileName = Path.GetFileName(photoUri);

                        var response = await httpClient.GetAsync(photoUri, HttpCompletionOption.ResponseHeadersRead);
                        response.EnsureSuccessStatusCode();

                        try
                        {
                            var responseStream = await response.Content.ReadAsStreamAsync();
                            if (responseStream == null || !responseStream.CanRead)
                                throw new Exception("Image cannot be downloaded");

                            var path = $".\\Images\\{d.ToString("yyyy-MM-dd")}-{fileName}";
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await responseStream.CopyToAsync(fileStream);
                            }
                        }
                        finally
                        {
                            response.Dispose();
                        }
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogError(e.Message);
                    this.logger.LogError(e.Source);
                    this.logger.LogError(e.StackTrace);

                    continue;
                }
            }

            return true;
        }
    }
}
