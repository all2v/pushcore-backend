using Microsoft.AspNetCore.Mvc;

namespace Astrum.Calendar.ViewModels;

public class EventForm
{
    [FromRoute]
    public string? Id { get; set; }
    public string CalendarId { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
}