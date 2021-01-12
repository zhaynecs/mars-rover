using System;
using System.Collections.Generic;

namespace MarsRover.Web.ViewModels
{
    public class DatedImagesViewModel
    {
        public DateTime TakenOn { get; set; }
        public IEnumerable<string> ImageUri { get; set; }
    }
}
