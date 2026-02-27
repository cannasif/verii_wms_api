using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class PlatformPageGroupMappingProfile : Profile
    {
        public PlatformPageGroupMappingProfile()
        {
            // PlatformPageGroup mappings
            CreateMap<PlatformPageGroup, PlatformPageGroupDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.GroupCode, opt => opt.MapFrom(src => src.GroupCode))
                .ForMember(dest => dest.MenuHeaderId, opt => opt.MapFrom(src => src.MenuHeaderId))
                .ForMember(dest => dest.MenuLineId, opt => opt.MapFrom(src => src.MenuLineId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.DeletedBy, opt => opt.MapFrom(src => src.DeletedBy))
                .ForMember(dest => dest.DeletedDate, opt => opt.MapFrom(src => src.DeletedDate))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.CreatedByFullUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? $"{src.CreatedByUser.FirstName} {src.CreatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.UpdatedByFullUser, opt => opt.MapFrom(src => src.UpdatedByUser != null ? $"{src.UpdatedByUser.FirstName} {src.UpdatedByUser.LastName}".Trim() : null))
                .ForMember(dest => dest.DeletedByFullUser, opt => opt.MapFrom(src => src.DeletedByUser != null ? $"{src.DeletedByUser.FirstName} {src.DeletedByUser.LastName}".Trim() : null));

            CreateMap<CreatePlatformPageGroupDto, PlatformPageGroup>()
                .ForMember(dest => dest.GroupCode, opt => opt.MapFrom(src => src.GroupCode))
                .ForMember(dest => dest.MenuHeaderId, opt => opt.MapFrom(src => src.MenuHeaderId))
                .ForMember(dest => dest.MenuLineId, opt => opt.MapFrom(src => src.MenuLineId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<UpdatePlatformPageGroupDto, PlatformPageGroup>()
                .ForMember(dest => dest.GroupCode, opt => opt.MapFrom(src => src.GroupCode))
                .ForMember(dest => dest.MenuHeaderId, opt => opt.MapFrom(src => src.MenuHeaderId))
                .ForMember(dest => dest.MenuLineId, opt => opt.MapFrom(src => src.MenuLineId))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }
}
