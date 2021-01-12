namespace MarsRover.Config.Models
{
    public interface IAppConfig
    {
        string Rover { get; set; }
        string DatesUri { get; set; }
        string ImagesUri { get; set; }
    }
}
