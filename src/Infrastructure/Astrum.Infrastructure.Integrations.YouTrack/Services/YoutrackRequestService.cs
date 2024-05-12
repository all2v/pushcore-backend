using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Newtonsoft.Json;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace Astrum.Infrastructure.Integrations.YouTrack.Services
{
    public class YoutrackRequestService : ITrackerRequestService
    {
        private readonly ILogHttpService _logHttpService;

        public YoutrackRequestService(ILogHttpService logHttpService)
        {
            _logHttpService = logHttpService;
        }

        public async Task<T> GetRequest<T>(string url, string token)
            where T : class
        {
            var auth = new Tuple<string, string>("Authorization", "Bearer " + token);
            var obj = await HttpHelper.GetAsync(url, headersExt: auth);
            var responseString = await obj.HttpResponseMessage.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(responseString);
            if (!obj.IsSuccess)
            {
                _logHttpService.Log<T>(null, result, url, "Запрос в ютрек", TypeRequest.GET, ModuleAstrum.TrackerProject);
            }
            
            return result;
        }

        private void HandleError(object? sender, ErrorEventArgs e)
        {
            var currentError = e.ErrorContext.Error.Message;
            e.ErrorContext.Handled = true;
        }
    }
}
