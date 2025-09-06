using MyBCA.Server.Models.Nutrislice;

namespace MyBCA.Server.Models.NewTab;

public record NewTabLunchTemplate(
    IEnumerable<MenuItem> MenuItems,
    DateTime? Expiry
);