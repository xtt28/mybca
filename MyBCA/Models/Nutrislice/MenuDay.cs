namespace MyBCA.Models.Nutrislice;

public record MenuDay(
    string? Date,
    IEnumerable<MenuItem> MenuItems
);