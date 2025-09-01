using MyBCA.Models;

namespace MyBCA.Services.Nutrislice
{
    public interface INutrisliceService
    {
        Task<MenuWeek> GetMenuWeekAsync();
        Task<MenuDay?> GetMenuDayAsync();
        DateTime? Expiry { get; }
    }
}