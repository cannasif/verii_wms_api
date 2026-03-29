using AutoMapper;
using Wms.Application.Production.Dtos;
using Wms.Domain.Entities.Production;

namespace Wms.Application.Production.Mappings;

public sealed class PrHeaderSerialMappingProfile : Profile
{
    public PrHeaderSerialMappingProfile()
    {
        CreateMap<PrHeaderSerial, PrHeaderSerialDto>();
        CreateMap<CreatePrHeaderSerialDto, PrHeaderSerial>();
        CreateMap<UpdatePrHeaderSerialDto, PrHeaderSerial>();
    }
}
