using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using Astrum.IdentityServer.DomainServices.ViewModels;
using AutoMapper;

namespace Astrum.IdentityServer.Application.Mappings
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            //CreateMap<UserViewModel, UserEditCommand>().ReverseMap();
            CreateMap<TokenOperationResult, UserTokenResult>().ReverseMap();
        }
    }
}
