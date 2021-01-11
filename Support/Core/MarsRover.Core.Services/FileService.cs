using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace MarsRover.Core.Services
{
    public class FileService: IFileService
    {
        private readonly ILogger<FileService> logger;

        public FileService(ILogger<FileService> logger)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<DateTime>> ReadAsync(IFormFile file)
        {
            var dates = new List<DateTime>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var line = (await reader.ReadLineAsync()).Trim();
                    DateTime d;

                    var success = DateTime.TryParse(line, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

                    if (!success)
                    {
                        this.logger.LogError($"{line} cannot be parsed into a date. Discarding data");
                        continue;
                    }

                    dates.Add(d);
                }
            }

            
            return dates;
        }
    }
}
