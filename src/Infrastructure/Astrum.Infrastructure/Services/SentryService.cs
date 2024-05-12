using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpRaven;
using SharpRaven.Data;

namespace Astrum.Infrastructure.Services
{
    public class SentryService
    {

        readonly IRavenClient _client;

        public SentryService(IRavenClient client)
        {
            _client = client;
        }

        public async Task SendEvent(Exception ex, string message = null, ErrorLevel level = ErrorLevel.Error, Dictionary<string, string> tags = null)
        {
            var sEvent = new SentryEvent(ex);
            if (message != null)
                sEvent.Message = new SentryMessage(message);
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    sEvent.Tags.Add(tag);
                }
            }
            var id = await _client.CaptureAsync(sEvent);
        }
    }
}
