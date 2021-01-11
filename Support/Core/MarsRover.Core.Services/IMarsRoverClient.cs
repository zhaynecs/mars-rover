using MarsRover.Core.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace MarsRover.Core.Services
{
    public interface IMarsRoverClient
    {
        IAsyncEnumerable<MarsRoverPhotoResponseViewModel> GetImagesAsync(DateTime date, int page = 1);
    }
}
