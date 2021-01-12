using MarsRover.Core.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace MarsRover.Core.Services
{
    public interface IMarsRoverService
    {
        IAsyncEnumerable<MarsRoverPhotoResponseViewModel> GetImagesAsync(string rover, DateTime date, int page = 1);
    }
}
