namespace MyBCA.Shared.Models.Links.Responses;

public record LinkApiResponse(int count, IEnumerable<Link> links);