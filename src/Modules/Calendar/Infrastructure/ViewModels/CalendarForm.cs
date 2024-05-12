using Microsoft.AspNetCore.Mvc;

namespace Astrum.Calendar.ViewModels;

public class CalendarForm
{
    [FromRoute]
    public string? CalendarId { get; set; }
    public string Summary { get; set; }
    public string? AccessRole { get; set; }
    public string? BackgroundColor { get; set; }
    public string? ForegroundColor { get; set; } = "#ffffff";
    public List<EventForm>? Events { get; set; }
}