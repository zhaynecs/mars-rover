using Newtonsoft.Json;
using System;

namespace MarsRover.Core.Models.ViewModels
{
    public class MarsRoverPhotoViewModel
    {
        [JsonProperty("img_src")]
        public string Uri { get; set; }
        [JsonProperty("earth_date")]
        public DateTime TakenOn { get; set; }
    }
}
