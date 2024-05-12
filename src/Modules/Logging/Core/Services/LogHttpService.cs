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
    public class LogHttpService: ILogHttpService
    {
        private readonly ILogger<LogHttpService> _logger;
        private readonly ILogHttpRepository _logRepository;
        private readonly IMapper _mapper;

        public LogHttpService(ILogger<LogHttpService> logger, ILogHttpRepository repository,
            IMapper mapper)
        {
            _logRepository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        private void WriteLog(LogLevel logLevel, LogHttp logData, ModuleAstrum module)
        {
            var logHttp = new LogHttp
            {
                LogLevel = logLevel,
                Module = module,
                BodyRequest = logData.BodyRequest,
                Description = logData.Description,
                RequestResponse = logData.RequestResponse,
                TypeRequest = logData.TypeRequest,
                Path = logData.Path,
                StatusCode = logData.StatusCode
            };
            try
            {
                _logger.CommonLogHttp(logLevel, logHttp);
            }
            catch (Exception ex)
            {
                //await _sentryService.SendEvent(ex, message: "Не удалось записать системный лог");
            }
        }

        public void Log<TViewModel>(object body, Result<TViewModel> result, HttpContext context, string description,
            TypeRequest typeRequest, ModuleAstrum module)
        {
            Log(body, result, context.Request.Path, description, typeRequest, module);
        }

        public void Log<TViewModel>(object body, Result<TViewModel> result, string path, string description,
            TypeRequest typeRequest, ModuleAstrum module)
        {
            Log(body, result.Data, path, description, typeRequest, module, result.Status, result.Errors, result.ValidationErrors);
        }

        public void Log<TViewModel>(object body, TViewModel result, string path, string description,
            TypeRequest typeRequest, ModuleAstrum module, 
            ResultStatus status, IEnumerable<string>? errors = null, List<ValidationError>? validationErrors = null)
        {
            LogHttp logDataHttp;
            switch (status)
            {
                case ResultStatus.Ok:
                    logDataHttp = new LogHttp
                    {
                        RequestResponse = JsonConvert.SerializeObject(result, Formatting.Indented),
                        BodyRequest = JsonConvert.SerializeObject(body, Formatting.Indented),
                        Description = description,
                        TypeRequest = typeRequest,
                        Path = path,
                        StatusCode = $"200 - {status}"
                    };
                    WriteLog(LogLevel.Information, logDataHttp, module);
                    break;
                case ResultStatus.Error:
                    logDataHttp = new LogHttp
                    {
                        RequestResponse = errors.First(),
                        BodyRequest = JsonConvert.SerializeObject(body, Formatting.Indented),
                        Description = $"{errors.Last()}",
                        TypeRequest = typeRequest,
                        Path = path,
                        StatusCode = $"422 - {status}"
                    };
                    WriteLog(LogLevel.Error, logDataHttp, module);
                    break;
                case ResultStatus.Forbidden:
                    break;
                case ResultStatus.Unauthorized:
                    break;
                case ResultStatus.Invalid:
                    var errorBuilder = new StringBuilder();
                    foreach (var error in validationErrors)
                    {
                        errorBuilder.Append(error.ErrorMessage);
                    }
                    logDataHttp = new LogHttp
                    {
                        RequestResponse = "Тело пустое.",
                        BodyRequest = JsonConvert.SerializeObject(body, Formatting.Indented),
                        Description = errorBuilder.ToString(),
                        TypeRequest = typeRequest,
                        Path = path,
                        StatusCode = $"400 - {status}"
                    };
                    WriteLog(LogLevel.Warning, logDataHttp, module);
                    break;
                case ResultStatus.NotFound:
                    logDataHttp = new LogHttp
                    {
                        RequestResponse = "Тело пустое.",
                        BodyRequest = JsonConvert.SerializeObject(body, Formatting.Indented),
                        Description = $"{errors.Last()}",
                        TypeRequest = typeRequest,
                        Path = path,
                        StatusCode = $"404 - {status}"
                    };
                    WriteLog(LogLevel.Error, logDataHttp, module);
                    break;
                default:
                    break;
            }
        }

        public async Task<IPagedList<LogHttpView>> GetLogs(LogFilter? logFilter = null, 
            int page = 1, int pageSize = ILogHttpService.PAGE_SIZE)
        {
            var spec = new GetFilteringHttpLogsSpec(logFilter);
            var logs = await _logRepository.PagedListAsync(page, pageSize, spec);
            var logResult = logs.ToMappedPagedList<LogHttp, LogHttpView>(_mapper, page, pageSize);
            return logResult;
        }
    }
}
