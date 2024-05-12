using Astrum.Identity.Models;
using Astrum.Logging.Entities;
using Astrum.Logging.Entities.LogEntities;
using Astrum.Logging.ViewModels;
using AutoMapper;

namespace Astrum.Logging.Mappings
{
    public class LogsProfile : Profile
    {
        public LogsProfile()
        {
            CreateMap<AbstractLog, CommonLogForm>();
            CreateMap<LogHttp, LogHttpView>().ReverseMap();
            CreateMap<LogAdmin, LogAdminView>().ReverseMap();

            CreateMap<ApplicationUser, UserLogs>().ReverseMap();
        }
    }
}
