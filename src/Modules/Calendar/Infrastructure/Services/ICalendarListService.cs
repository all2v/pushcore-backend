using Astrum.Calendar.Services;
using Astrum.Calendar.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Calendar.Infrastructure.Services;

public interface ICalendarListService : IGoogleCalendar
{
    Task<IEnumerable<CalendarForm>> GetCalendarListAsync();
    Task<CalendarForm> GetCalendarAsync(string id);
    Task<Result<CalendarForm>> CreateCalendarAsync(CalendarForm form);
    Task<Result<CalendarForm>> UpdateCalendarAsync(CalendarForm form);
    Task<Result<CalendarForm>> DeleteCalendarAsync(string id);
}