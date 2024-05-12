using Astrum.Account.Features.Profile;
using Astrum.Debts.Application.Models.CreateModels;
using Astrum.Debts.Application.Models.UpdateModels;
using Astrum.Debts.Application.Models.ViewModels;
using Astrum.Debts.Domain.Aggregates;
using AutoMapper;

namespace Astrum.Debts.Application.Mappings
{
    public class DebtProfile : Profile
    {
        public DebtProfile()
        {
            CreateMap<Debt, DebtView>().ReverseMap();
            CreateMap<Debt, DebtCreateRequest>().ReverseMap();
            CreateMap<Debt, DebtUpdateRequest>().ReverseMap();

            CreateMap<UserProfileSummary, UserDebt>().ReverseMap();
        }
    }
}
