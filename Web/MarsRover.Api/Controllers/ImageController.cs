using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarsRover.Api.ViewModels;
using MarsRover.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MarsRover.Api.Controllers
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
        public async Task<IActionResult> DownloadImagesAsync([FromForm]IFormFile file)
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

            var success = await this.imageService.DownloadMarsRoverImagesAsync("curiosity", dates);

            return Ok(success);
        }

    }
}
