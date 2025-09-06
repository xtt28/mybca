namespace MyBCA.Server.Services.Nutrislice;

public class NutrisliceOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public TimeSpan CacheTtl { get; set; } = TimeSpan.Zero;
}
