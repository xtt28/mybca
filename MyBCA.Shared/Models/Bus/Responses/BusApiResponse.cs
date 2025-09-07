namespace MyBCA.Shared.Models.Bus.Responses;

public record BusApiResponse(int Count, Dictionary<string, string> Locations, DateTime? Expiry);