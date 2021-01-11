using MarsRover.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public interface IImageService
    {
        Task<IEnumerable<Image>> GetMarsRoverImagesAsync(IEnumerable<DateTime> dates);
    }
}
