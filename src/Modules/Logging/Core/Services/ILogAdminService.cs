using Astrum.Logging.Entities;
using Astrum.Logging.ViewModels;
using Astrum.Logging.ViewModels.Filters;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Sakura.AspNetCore;

namespace Astrum.Logging.Services
{
    public interface ILogAdminService
    {
        protected const int PAGE_SIZE = 10;
        public Task<IPagedList<LogAdminView>> GetLogs(LogFilterAdmin logFilter, int page = 1, int pageSize = PAGE_SIZE);
        public void Log<TViewModel>(object bodyRequest, Result<TViewModel> result, string description, ModuleAstrum module);
    }
}
