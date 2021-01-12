using System;

namespace MarsRover.Config.Models
{
    public interface IHttpClientConfig
    {
        Uri Url { get; set; }
        int Timeout { get; set; }
    }
}
