using MyBCA.Server.Models.Nutrislice;

namespace MyBCA.Server.Services.Nutrislice;

public interface INutrisliceService
{
    Task<MenuWeek> GetMenuWeekAsync();
    Task<MenuDay?> GetMenuDayAsync();
    DateTime? Expiry { get; }
}
