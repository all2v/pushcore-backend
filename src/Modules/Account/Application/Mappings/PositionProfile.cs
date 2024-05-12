using Astrum.Account.Domain.Aggregates;
using Astrum.Account.ViewModels;
using AutoMapper;

namespace Astrum.Account.Application.Mappings
{
    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            CreateMap<Position, PositionForm>();
            CreateMap<PositionForm, Position>();
        }
    }
}
