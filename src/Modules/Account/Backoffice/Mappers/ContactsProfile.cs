using Astrum.Account.Features;
using Astrum.Account.Features.Profile.Commands;
using AutoMapper;

namespace Astrum.Account.Mappers;

public class ContactsProfile : Profile
{
    public ContactsProfile()
    {
        CreateMap<EditContactsRequestBody, EditContactsCommand>();
    }
}