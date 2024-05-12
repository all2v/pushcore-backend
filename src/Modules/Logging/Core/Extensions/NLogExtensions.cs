using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Logging.Entities;
using Microsoft.Extensions.Logging;

namespace Astrum.Logging.Extensions
{
    public static class NLogExtensions
    {
        public static void CommonLogHttp<T>(this ILogger logger, LogLevel logLevel, T logData) where T : LogHttp
        {
            var state = new LogEvent("")
                .WithProperty("StatusCode", logData.StatusCode)
                .WithProperty("TypeRequest", logData.TypeRequest)
                .WithProperty("Path", logData.Path)
                .WithProperty("Description", logData.Description)
                .WithProperty("BodyRequest", logData.BodyRequest)
                .WithProperty("RequestResponse", logData.RequestResponse)
                .WithProperty("Module", logData.Module);

            logger.Log(logLevel, default, state, null, LogEvent.Formatter);
        }

        public static void CommonLogAdmin<T>(this ILogger logger, LogLevel logLevel, T logData) where T : LogAdmin
        {
            var state = new LogEvent("")
                .WithProperty("Description", logData.Description)
                .WithProperty("BodyRequest", logData.BodyRequest)
                .WithProperty("RequestResponse", logData.RequestResponse)
                .WithProperty("Status", logData.Status)
                .WithProperty("Module", logData.Module);

            logger.Log(logLevel, default, state, null, LogEvent.Formatter);
        }
    }

    public class LogEvent : IEnumerable<KeyValuePair<string, object>>
    {
        List<KeyValuePair<string, object>> _properties = new List<KeyValuePair<string, object>>();

        public string Message { get; }

        public LogEvent(string message)
        {
            Message = message;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        public LogEvent WithProperty(string name, object value)
        {
            _properties.Add(new KeyValuePair<string, object>(name, value));
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static Func<LogEvent, Exception, string> Formatter { get; } = (l, e) => l.Message;
    }
}
