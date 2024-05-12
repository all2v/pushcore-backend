using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrum.Infrastructure.Integrations.YouTrack.Services
{
    public interface ITrackerRequestService
    {
        Task<T> GetRequest<T>(string url, string token) where T : class;
    }
}
