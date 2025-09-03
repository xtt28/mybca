using MyBCA.Models.Nutrislice;

namespace MyBCA.Models.NewTab;

public record NewTabLunchTemplate(
    IEnumerable<MenuItem> MenuItems,
    DateTime? Expiry
);