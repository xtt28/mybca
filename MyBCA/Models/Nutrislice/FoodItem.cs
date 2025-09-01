namespace MyBCA.Models.Nutrislice;

public record FoodItem(
    int Id,
    string? Name,
    string? Description,
    string? Subtext,
    string? ImageUrl
);