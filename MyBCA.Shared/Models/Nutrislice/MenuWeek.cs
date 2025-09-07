namespace MyBCA.Shared.Models.Nutrislice;

public record MenuWeek(
    string? StartDate,
    string? DisplayName,
    IEnumerable<MenuDay> Days
);