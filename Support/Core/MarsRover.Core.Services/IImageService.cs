using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public interface IImageService
    {
        Task<bool> DownloadMarsRoverImagesAsync(string rover, IEnumerable<DateTime> dates);
    }
}
