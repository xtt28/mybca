namespace MyBCA.Shared.Models.Nutrislice;

public record MenuItem(
    DateTime? Date,
    int Position,
    bool IsSectionTitle,
    string? Text,
    FoodItem Food,
    uint StationID,
    bool IsStationHeader,
    string? Image,
    string? Category
);