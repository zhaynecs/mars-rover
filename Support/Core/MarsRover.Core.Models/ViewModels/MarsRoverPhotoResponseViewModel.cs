using Newtonsoft.Json;
using System.Collections.Generic;

namespace MarsRover.Core.Models.ViewModels
{
    public class MarsRoverPhotoResponseViewModel
    {
        [JsonProperty("photos")]
        public IEnumerable<MarsRoverPhotoViewModel> Photos { get; set; }
    }
}
