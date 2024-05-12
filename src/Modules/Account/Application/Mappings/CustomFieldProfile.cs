using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Aggregates;
using Astrum.Account.Features.CustomField;
using AutoMapper;

namespace Astrum.Account.Mappings
{
    public class CustomFieldProfile : Profile
    {
        public CustomFieldProfile() 
        {
            CreateMap<CustomField, CustomFieldResponse>();
        }
    }
}
