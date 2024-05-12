using Astrum.Logging.Entities;
using Astrum.Logging.ViewModels;
using Astrum.Logging.ViewModels.Filters;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Sakura.AspNetCore;

namespace Astrum.Logging.Services
{
    public interface ILogHttpService
    {
        protected const int PAGE_SIZE = 10;
        public Task<IPagedList<LogHttpView>> GetLogs(LogFilter? filter, int page = 1, int pageSize = PAGE_SIZE);
        public void Log<TViewModel>(object body, Result<TViewModel> result, HttpContext context, string description, 
            TypeRequest typeRequest, ModuleAstrum module);
        public void Log<TViewModel>(object body, Result<TViewModel> result, string path, string description,
            TypeRequest typeRequest, ModuleAstrum module);

        void Log<TViewModel>(object body, TViewModel result, string path, string description,
            TypeRequest typeRequest, ModuleAstrum module,
            ResultStatus status, IEnumerable<string>? errors = null, List<ValidationError>? validationErrors = null);
    }
}
