using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Astrum.Articles.Aggregates;
using Astrum.Articles.ViewModels;
using Astrum.Module.Articles.Application.ViewModels;
using AutoMapper;

namespace Astrum.Articles.Mappings
{
    public class AuthorProfie:Profile
    {
        public AuthorProfie()
        {
            CreateMap<Author, AuthorView>();
            
            CreateMap<UserProfileSummary, AuthorView> ();
        }
    }
}
