using AutoMapper;
using Wms.Application.SubcontractingReceiptTransfer.Dtos;
using Wms.Domain.Entities.SubcontractingReceiptTransfer.Functions;

namespace Wms.Application.SubcontractingReceiptTransfer.Mappings;

public sealed class SrtFunctionMappingProfile : Profile
{
    public SrtFunctionMappingProfile()
    {
        CreateMap<FnSrtOpenOrderHeader, SrtOpenOrderHeaderDto>();
        CreateMap<FnSrtOpenOrderLine, SrtOpenOrderLineDto>();
    }
}
