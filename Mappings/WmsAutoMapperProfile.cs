using AutoMapper;
using WMS_WEBAPI.DTOs;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Mappings
{
    public class WmsAutoMapperProfile : Profile
    {
        public WmsAutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.RoleNavigation != null ? src.RoleNavigation.Title : string.Empty))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));
            
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));

            // UserDetail mappings
            CreateMap<UserDetail, UserDetailDto>()
                .ApplyFullUserNames();
            
            CreateMap<CreateUserDetailDto, UserDetail>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<UpdateUserDetailDto, UserDetail>()
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<SmtpSetting, SmtpSettingsDto>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedDate ?? src.CreatedDate ?? DateTime.UtcNow));

            CreateMap<UpdateSmtpSettingsDto, SmtpSetting>()
                .ForMember(dest => dest.PasswordEncrypted, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.DeletedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }

    public static class MappingExtensions
    {
        private static string? FullName(User? user)
        {
            return user != null ? ($"{user.FirstName} {user.LastName}").Trim() : null;
        }

        public static IMappingExpression<TSource, TDestination> ApplyFullUserNames<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expr)
            where TSource : BaseEntity
            where TDestination : BaseEntityDto
        {
            expr.ForMember(nameof(BaseEntityDto.CreatedByFullUser), opt => opt.MapFrom(src => FullName(src.CreatedByUser)));
            expr.ForMember(nameof(BaseEntityDto.UpdatedByFullUser), opt => opt.MapFrom(src => FullName(src.UpdatedByUser)));
            expr.ForMember(nameof(BaseEntityDto.DeletedByFullUser), opt => opt.MapFrom(src => FullName(src.DeletedByUser)));
            return expr;
        }
    }
}
