using Astrum.Account.Features.Profile;
using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Domain.Aggregates;
using AutoMapper;

namespace Astrum.Inventory.Application.Mappings
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Characteristic, CharacteristicView>().ReverseMap();
            CreateMap<Characteristic, CharacteristicCreateRequest>().ReverseMap();
            CreateMap<Characteristic, CharacteristicUpdateRequest>().ReverseMap();
            CreateMap<CharacteristicView, CharacteristicCreateRequest>().ReverseMap();
            CreateMap<CharacteristicView, CharacteristicUpdateRequest>().ReverseMap();

            CreateMap<InventoryItem, InventoryItemView>().ReverseMap();
            CreateMap<InventoryItem, InventoryItemCreateRequest>().ReverseMap();
            CreateMap<InventoryItem, InventoryItemUpdateRequest>().ReverseMap();
            CreateMap<InventoryItemView, InventoryItemUpdateRequest>().ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Template, TemplateView>().ReverseMap();
            CreateMap<Template, TemplateCreateRequest>().ReverseMap();
            CreateMap<Template, TemplateUpdateRequest>().ReverseMap();
            CreateMap<TemplateView, TemplateUpdateRequest>().ReverseMap();

            CreateMap<UserProfileSummary, UserInventory>().ReverseMap();
        }
    }
}
