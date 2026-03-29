using AutoMapper;
using Wms.Application.Production.Dtos;
using Wms.Domain.Entities.Production.Functions;

namespace Wms.Application.Production.Mappings;

public sealed class PrFunctionMappingProfile : Profile
{
    public PrFunctionMappingProfile()
    {
        CreateMap<FnProductHeader, ProductHeaderDto>()
            .ForMember(d => d.IsEmriNo, o => o.MapFrom(s => s.ISEMRINO))
            .ForMember(d => d.StockCode, o => o.MapFrom(s => s.STOK_KODU))
            .ForMember(d => d.StockName, o => o.MapFrom(s => s.STOK_ADI))
            .ForMember(d => d.YapKod, o => o.MapFrom(s => s.YAPKOD))
            .ForMember(d => d.YapAcik, o => o.MapFrom(s => s.YAPACIK))
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.MIKTAR))
            .ForMember(d => d.Priority, o => o.MapFrom(s => s.ONCELIK))
            .ForMember(d => d.RefIsEmriNo, o => o.MapFrom(s => s.REFISEMRINO));
        CreateMap<FnProductLine, ProductLineDto>()
            .ForMember(d => d.MamulKodu, o => o.MapFrom(s => s.MAMUL_KODU))
            .ForMember(d => d.MamulAdi, o => o.MapFrom(s => s.MAMUL_ADI))
            .ForMember(d => d.IsEmriNo, o => o.MapFrom(s => s.ISEMRINO))
            .ForMember(d => d.SiparisNo, o => o.MapFrom(s => s.SIPARIS_NO))
            .ForMember(d => d.HamKodu, o => o.MapFrom(s => s.HAM_KODU))
            .ForMember(d => d.HamMaddeAdi, o => o.MapFrom(s => s.HAM_MADDE_ADI))
            .ForMember(d => d.BirimMiktar, o => o.MapFrom(s => s.BIRIM_MIKTAR))
            .ForMember(d => d.HesaplananToplamMiktar, o => o.MapFrom(s => s.HESAPLANAN_TOPLAM_MIKTAR))
            .ForMember(d => d.OpNo, o => o.MapFrom(s => s.OPNO));
    }
}
