namespace MyBCA.Models.Nutrislice;

public record MenuWeek(
    string? StartDate,
    string? DisplayName,
    IEnumerable<MenuDay> Days
);