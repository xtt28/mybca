namespace MyBCA.Services.Nutrislice
{
    public class NutrisliceOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public TimeSpan CacheTTL { get; set; } = TimeSpan.Zero;
    }
}