namespace MyBCA.Server.Services.Bus;

public class BusOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public TimeSpan CacheTtlNormal { get; set; } = TimeSpan.Zero;
    public TimeSpan CacheTtlDismissalTime { get; set; } = TimeSpan.Zero;
}
