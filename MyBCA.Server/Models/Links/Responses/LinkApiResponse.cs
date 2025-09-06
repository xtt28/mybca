namespace MyBCA.Server.Models.Links.Responses;

public record LinkApiResponse(int count, IEnumerable<Link> links);