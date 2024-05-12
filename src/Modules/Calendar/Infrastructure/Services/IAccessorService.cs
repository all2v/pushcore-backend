using Google.Apis.Calendar.v3;

namespace Astrum.Calendar.Services;

public interface IAccessorService
{
    Task<CalendarService> GetAccess(CancellationToken cancellationToken);
}