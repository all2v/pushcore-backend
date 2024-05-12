using Astrum.Calendar.ViewModels;

namespace Astrum.Calendar.GraphQL;

public class SubscriptionCalendar
{
    [Subscribe]
    public IEnumerable<EventForm> EventsChanged([EventMessage] IEnumerable<EventForm> events)
    {
        return events;
    }

    [Subscribe]
    public IEnumerable<CalendarForm> CalendarsChanged([EventMessage] IEnumerable<CalendarForm> calendarList)
    {
        return calendarList;
    }
}