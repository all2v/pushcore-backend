using Astrum.Account.Features.Profile;
using Astrum.News.Aggregates;
using Astrum.News.Application.ViewModels.Requests;
using Astrum.News.ViewModels;
using AutoMapper;
using Mono.Linq.Expressions;

namespace Astrum.News.Mappings;

public class NewsBackofficeProfile : Profile
{
    public NewsBackofficeProfile()
    {
        CreateMap<UserInfo, UserProfileSummary>();
        CreateMap<UserProfileSummary, UserInfo>();
    }
}