using Astrum.Identity.Application.ViewModels;
using Astrum.Identity.Features.Commands;
using Astrum.Identity.Features.Commands.Register;
using Astrum.Identity.Models;
using AutoMapper;

namespace Astrum.Identity.Mappings;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterCommand, CreateUserCommand>()
            .ForMember(e => e.Name, opt => opt.MapFrom(m => $"{m.Surname} {m.Name} {m.Patronymic}"));
        CreateMap<ApplicationUser, TokenGenerationForm>()
            .ForMember(e => e.Id, opt => opt.MapFrom(m => m.Id.ToString()));
    }
}