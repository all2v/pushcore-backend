using Astrum.Telegram.Application.ViewModels;
using Astrum.Telegram.Domain.Aggregates;
using AutoMapper;

namespace Astrum.Telegram.Mappings
{
    public class TelegramProfile : Profile
    {
        public TelegramProfile()
        {
            CreateMap<TelegramChat, ChatForm>().ReverseMap();
        }
    }
}
