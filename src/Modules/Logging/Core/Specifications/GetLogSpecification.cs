using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Astrum.Logging.Entities;
using Astrum.Logging.ViewModels.Filters;

namespace Astrum.Logging.Specifications
{
    public class GetLogSpecification<TEntity> : Specification<TEntity>
    where TEntity : AbstractLog
    {
        public GetLogSpecification()
        {
            Query.OrderByDescending(x => x.DateCreated);
        }
    }

    public class GetHttpLogsSpec : Specification<LogHttp> {
    }

    public class GetHttpLogsByModuleSpec : GetHttpLogsSpec
    {
        public GetHttpLogsByModuleSpec(ModuleAstrum module)
        {
            Query
                .Where(logs => logs.Module == module)
                .OrderByDescending(x => x.DateCreated);
        }
    }

    public class GetFilteringHttpLogsSpec : GetHttpLogsSpec
    {
        public GetFilteringHttpLogsSpec(LogFilter? logFilter)
        {
            Query
                .Where(logs => logFilter.Module == null || logs.Module == logFilter.Module)
                .Where(logs => logFilter.LogLevel == null || logs.LogLevel == logFilter.LogLevel)
                .Where(logs => logFilter.BeginPeriod == null || logs.DateCreated >= logFilter.BeginPeriod)
                .Where(logs => logFilter.EndPeriod == null || logs.DateCreated <= logFilter.EndPeriod)
                .Where(logs => logFilter.StatusCode == null || logs.StatusCode.ToLower().Contains(logFilter.StatusCode.ToLower()))
                .Where(logs => logFilter.TypeRequest == null || logs.TypeRequest == logFilter.TypeRequest)
                .Where(logs => logFilter.Description == null || logs.Description.ToLower().Contains(logFilter.Description.ToLower()))
                .Where(logs => logFilter.Path == null || logs.Path.ToLower().Contains(logFilter.Path.ToLower()))
                .Where(logs => logFilter.BodyRequest == null || logs.BodyRequest.ToLower().Contains(logFilter.BodyRequest.ToLower()))
                .Where(logs => logFilter.RequestResponse == null || logs.RequestResponse.ToLower().Contains(logFilter.RequestResponse.ToLower()))
                .OrderByDescending(x => x.DateCreated);
        }
    }

    public class GetAdminLogsSpec : Specification<LogAdmin>
    {

    }

    public class GetAdminLogsByModuleSpec : GetAdminLogsSpec
    {
        public GetAdminLogsByModuleSpec(ModuleAstrum module)
        {

            Query.Where(logs => logs.Module == module)
                .OrderByDescending(x => x.DateCreated);
        }
    }

    public class GetFilteringAdminLogsSpec : GetAdminLogsSpec
    {
        public GetFilteringAdminLogsSpec(LogFilterAdmin? logFilter)
        {
            Query
                .Where(logs => logFilter.Module == null || logs.Module == logFilter.Module)
                .Where(logs => logFilter.LogLevel == null || logs.LogLevel == logFilter.LogLevel)
                .Where(logs => logFilter.BeginPeriod == null || logs.DateCreated >= (DateTimeOffset)logFilter.BeginPeriod)
                .Where(logs => logFilter.EndPeriod == null || logs.DateCreated <= (DateTimeOffset)logFilter.EndPeriod)
                .Where(logs => logFilter.Status == null || logs.Status == logFilter.Status)
                .Where(logs => logFilter.Description == null || logs.Description.ToLower().Contains(logFilter.Description.ToLower()))
                .Where(logs => logFilter.BodyRequest == null || logs.BodyRequest.ToLower().Contains(logFilter.BodyRequest.ToLower()))
                .Where(logs => logFilter.RequestResponse == null || logs.RequestResponse.ToLower().Contains(logFilter.RequestResponse.ToLower()))
                .OrderByDescending(x => x.DateCreated);
        }
    }
}
