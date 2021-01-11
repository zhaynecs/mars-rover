namespace MarsRover.Config.Models
{
    public interface IRetryPolicyConfig
    {
        int RetryCount { get; set; }
    }
}
