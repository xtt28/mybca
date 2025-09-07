namespace MyBCA.Shared.Models.Links.Responses;

public record LinkApiResponse(int Count, IEnumerable<Link> Data);