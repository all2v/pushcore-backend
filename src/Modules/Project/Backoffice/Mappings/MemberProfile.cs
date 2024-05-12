using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Astrum.Projects.Aggregates;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using AutoMapper;

namespace Astrum.Projects.Mappings
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            
            CreateMap<UserProfileSummary, MemberView>().ForMember(e=>e.Role, opt => opt.Ignore());
        }
    }
}
