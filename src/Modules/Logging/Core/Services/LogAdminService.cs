using System.Text;
using Astrum.Logging.Entities;
using Astrum.Logging.Extensions;
using Astrum.Logging.Repositories;
using Astrum.Logging.Specifications;
using Astrum.Logging.ViewModels;
using Astrum.Logging.ViewModels.Filters;
using Astrum.SharedLib.Application.Extensions;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sakura.AspNetCore;

namespace Astrum.Logging.Services
{
    public class LogAdminService : ILogAdminService
    {
        private readonly ILogger<LogAdminService> _logger;
        private readonly ILogAdminRepository _logRepository;
        private readonly IMapper _mapper;

        public LogAdminService(ILogger<LogAdminService> logger, ILogAdminRepository repository,
            IMapper mapper)
        {
            _logRepository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        private void WriteLog(LogLevel logLevel, LogAdmin logData, ModuleAstrum module)
        {
            var logAdmin = new LogAdmin
            {
                LogLevel = logLevel,
                Module = module,
                BodyRequest = logData.BodyRequest,
                Description = logData.Description,
                RequestResponse = logData.RequestResponse,
                Status = logData.Status,
            };
            try
            {
                _logger.CommonLogAdmin(logLevel, logAdmin);
            }
            catch (Exception ex)
            {
                //await _sentryService.SendEvent(ex, message: "Не удалось записать системный лог");
            }
        }

        public void Log<TViewModel>(object bodyRequest, Result<TViewModel> result, string description, ModuleAstrum module)
        {
            LogAdmin logAdmin;
            switch (result.Status)
            {
                case ResultStatus.Ok:
                    logAdmin = new LogAdmin
                    {
                        RequestResponse = JsonConvert.SerializeObject(result.Data, Formatting.Indented),
                        BodyRequest = JsonConvert.SerializeObject(bodyRequest, Formatting.Indented),
                        Description = description,
                        Status = result.Status
                    };
                    WriteLog(LogLevel.Information, logAdmin, module);
                    break;
                case ResultStatus.Error:
                    logAdmin = new LogAdmin
                    {
                        RequestResponse = result.Errors.First(),
                        BodyRequest = JsonConvert.SerializeObject(bodyRequest, Formatting.Indented),
                        Description = $"{result.Errors.Last()}",
                        Status = result.Status
                    };
                    WriteLog(LogLevel.Error, logAdmin, module);
                    break;
                case ResultStatus.Forbidden:
                    break;
                case ResultStatus.Unauthorized:
                    break;
                case ResultStatus.Invalid:
                    StringBuilder errors = new StringBuilder();
                    foreach (var error in result.ValidationErrors)
                    {
                        errors.Append(error.ErrorMessage);
                    }
                    logAdmin = new LogAdmin
                    {
                        RequestResponse = "Тело пустое.",
                        BodyRequest = JsonConvert.SerializeObject(bodyRequest, Formatting.Indented),
                        Description = errors.ToString(),
                        Status = result.Status
                    };
                    WriteLog(LogLevel.Warning, logAdmin, module);
                    break;
                case ResultStatus.NotFound:
                    logAdmin = new LogAdmin
                    {
                        RequestResponse = "Тело пустое.",
                        BodyRequest = JsonConvert.SerializeObject(bodyRequest, Formatting.Indented),
                        Description = $"{result.Errors.Last()}",
                        Status = result.Status
                    };
                    WriteLog(LogLevel.Error, logAdmin, module);
                    break;
                default:
                    break;
            }
        }

        public async Task<IPagedList<LogAdminView>> GetLogs(LogFilterAdmin? logFilter = null,
            int page = 1, int pageSize = ILogAdminService.PAGE_SIZE)
        {
            var spec = new GetFilteringAdminLogsSpec(logFilter);
            var logs = await _logRepository.PagedListAsync(page, pageSize, spec);
            var logResult = logs.ToMappedPagedList<LogAdmin, LogAdminView>(_mapper, page, pageSize);
            return logResult;
        }

    }
}
