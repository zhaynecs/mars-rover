using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarsRover.Core.Services;
using MarsRover.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarsRover.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ILogger<ImageController> logger;

        private readonly IFileService fileService;
        private readonly IImageService imageService;

        public ImageController(ILogger<ImageController> logger, IFileService fileService, IImageService imageService)
        {
            this.logger = logger;

            this.fileService = fileService;
            this.imageService = imageService;
        }

        [HttpGet]
        public async Task<IActionResult> DownloadImagesAsync([FromForm] IFormFile file)
        {
            if (file == null)
            {
                this.logger.LogError("No file was uploaded");
                return BadRequest("No file was uploaded");
            }

            if (file.Length == 0)
            {
                this.logger.LogError($"File {file.FileName} is empty");
                return BadRequest($"File {file.FileName} is empty");
            }

            var fileType = Path.GetExtension(file.FileName);
            if (fileType.ToLower() != ".txt")
            {
                this.logger.LogError($"A *{fileType} file was uploaded. Only Only text files (*.txt) are supported.");
                return BadRequest("Only text files (*.txt) are supported");
            }

            var dates = await this.fileService.ReadAsync(file);
            if (dates == null || !dates.Any())
            {
                this.logger.LogError($"File {file.FileName} does not contain any valid dates");
                return BadRequest("File does not contain any valid dates");
            }

            var images = await this.imageService.DownloadMarsRoverImagesAsync(dates);
            var viewModels = new List<DatedImagesViewModel>();
            if (images == null || !images.Any())
                return Ok(viewModels);

            viewModels = images.GroupBy(i => i.TakenOn)
                .Select(g => new DatedImagesViewModel
                {
                    TakenOn = g.Key,
                    ImageUri = g.Select(gi => gi.Uri)
                }).ToList();

            return Ok(viewModels);
        }

    }
}
