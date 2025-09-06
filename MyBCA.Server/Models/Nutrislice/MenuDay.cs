namespace MyBCA.Server.Models.Nutrislice;

public record MenuDay(
    string? Date,
    IEnumerable<MenuItem> MenuItems
);