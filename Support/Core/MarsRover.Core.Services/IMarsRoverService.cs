using MarsRover.Core.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public interface IMarsRoverService
    {
        Task<MarsRoverPhotoResponseViewModel> GetImageDataAsync(string rover, DateTime date, int page = 1);
        IAsyncEnumerable<MarsRoverPhotoResponseViewModel> GetImageDataStreamAsync(string rover, DateTime date, int page = 1);
    }
}
