using MarsRover.Core.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public class ImageService: IImageService
    {
        private readonly ILogger<ImageService> logger;

        private readonly IMarsRoverClient marsRoverApiService;
        public ImageService(ILogger<ImageService> logger, IMarsRoverClient marsRoverApiService)
        {
            this.logger = logger;
            this.marsRoverApiService = marsRoverApiService;
        }

        public async Task<IEnumerable<Image>> GetMarsRoverImagesAsync(IEnumerable<DateTime> dates)
        {
            if (dates == null || !dates.Any())
                throw new ArgumentException("No dates provided");

            var images = new List<Image>();
            foreach(var d in dates)
            {
                try
                {
                    await foreach (var r in this.marsRoverApiService.GetImagesAsync(d))
                    {
                        if (r == null || r.Photos == null || !r.Photos.Any())
                        {
                            this.logger.LogError($"No images were found on {d.Date}");
                            continue;
                        }

                        var photos = r.Photos;
                        images.AddRange(photos.Select(p => new Image
                        {
                            Uri = p.Uri,
                            TakenOn = p.TakenOn
                        }));
                    }
                }
                catch(Exception e)
                {
                    this.logger.LogError(e.Message);
                    this.logger.LogError(e.Source);
                    this.logger.LogError(e.StackTrace);

                    continue;
                }
            }

            return images;
        }
    }
}
