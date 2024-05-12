namespace Astrum.Calendar.Services;

public interface IGoogleCalendar
{
    void GetAccess(CancellationToken cancellationToken);
}