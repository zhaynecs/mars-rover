﻿using System;

namespace MarsRover.Config.Models
{
    public class HttpClientConfig
    {
        public Uri Url { get; set; }
        public int Timeout { get; set; }
    }
}
