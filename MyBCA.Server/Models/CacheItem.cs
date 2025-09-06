namespace MyBCA.Server.Models;

public class CacheItem<T>
{
    public required T Value { get; set; }
    public DateTime Expiry { get; set; }
}