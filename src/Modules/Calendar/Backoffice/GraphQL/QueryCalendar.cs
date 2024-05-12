using Astrum.Calendar.Infrastructure.Services;
using Astrum.Calendar.Services;
using Astrum.Calendar.ViewModels;

namespace Astrum.Calendar.GraphQL;

public class QueryCalendar
{
    [UseSorting]
    [UseFiltering]
    public async Task<IEnumerable<CalendarForm>> GetCalendarList([Service] ICalendarListService calendarListService)
    {
        return await calendarListService.GetCalendarListAsync();
    }

    public async Task<CalendarForm> GetCalendar(string id, [Service] ICalendarListService calendarListService)
    {
        return await calendarListService.GetCalendarAsync(id);
    }

    [UseSorting]
    [UseFiltering]
    public async Task<IEnumerable<EventForm>> GetEvents(string calendarId, [Service] IEventService eventService)
    {
        return await eventService.GetEventsAsync(calendarId);
    }

    public async Task<EventForm> GetEvent(string calendarId, string id, [Service] IEventService eventService)
    {
        return await eventService.GetEventAsync(calendarId, id);
    }
}