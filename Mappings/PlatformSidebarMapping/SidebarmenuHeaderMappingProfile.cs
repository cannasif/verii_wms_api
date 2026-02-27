using AutoMapper;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.DTOs;

namespace WMS_WEBAPI.Mappings
{
    public class SidebarmenuHeaderMappingProfile : Profile
    {
        public SidebarmenuHeaderMappingProfile()
        {
            // Entity to DTO
            CreateMap<SidebarmenuHeader, SidebarmenuHeaderDto>()
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            // DTO to Entity
            CreateMap<SidebarmenuHeaderDto, SidebarmenuHeader>();

            CreateMap<CreateSidebarmenuHeaderDto, SidebarmenuHeader>()
                .ForMember(dest => dest.MenuKey, opt => opt.MapFrom(src => src.MenuKey))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
                .ForMember(dest => dest.DarkColor, opt => opt.MapFrom(src => src.DarkColor))
                .ForMember(dest => dest.ShadowColor, opt => opt.MapFrom(src => src.ShadowColor))
                .ForMember(dest => dest.DarkShadowColor, opt => opt.MapFrom(src => src.DarkShadowColor))
                .ForMember(dest => dest.TextColor, opt => opt.MapFrom(src => src.TextColor))
                .ForMember(dest => dest.DarkTextColor, opt => opt.MapFrom(src => src.DarkTextColor))
                .ForMember(dest => dest.RoleLevel, opt => opt.MapFrom(src => src.RoleLevel))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Lines, opt => opt.Ignore());
        }
    }
}