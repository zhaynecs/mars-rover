using System;
using System.Collections.Generic;
using System.Text;

namespace MarsRover.Config.Models
{
    public class AppConfig: IAppConfig
    {
        private string _rover;
        public string Rover { 
            get
            {
                return this._rover;
            }
            set
            {
                this._rover = value.ToLower();
            }
        }

        public string DatesUri { get; set; }
        public string ImagesUri { get; set; }
    }
}
