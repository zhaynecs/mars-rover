namespace MarsRover.Config.Models
{
    public class HttpPolicyConfig: IRetryPolicyConfig
    {
        public int RetryCount { get; set; }
    }
}
