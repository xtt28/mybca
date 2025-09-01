namespace MyBCA.Models.Nutrislice;

public record FoodItem(
    int Id,
    string? Name,
    string? Description,
    string? Subtext,
    string? ImageUrl
);

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

public record MenuDay(
    string? Date,
    IEnumerable<MenuItem> MenuItems
);

public record MenuWeek(
    string? StartDate,
    string? DisplayName,
    IEnumerable<MenuDay> Days
);
