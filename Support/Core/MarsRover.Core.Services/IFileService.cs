using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public interface IFileService
    {
        Task<IEnumerable<DateTime>> ReadAsync(IFormFile file);
        Task<IEnumerable<DateTime>> ReadAsync(string filePath);
    }
}
