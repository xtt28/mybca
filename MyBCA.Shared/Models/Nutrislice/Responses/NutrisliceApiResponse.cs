namespace MyBCA.Shared.Models.Nutrislice.Responses;

public record NutrisliceApiResponse<T>(T Data, DateTime? Expiry);