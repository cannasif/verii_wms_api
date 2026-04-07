using Wms.Domain.Entities.Common;

namespace Wms.Application.Common;

public interface IAssignedBarcodeMatchingService
{
    Task<AssignedBarcodeMatchResult> MatchAsync<TLine, TLineSerial>(
        AssignedBarcodeMatchRequest<TLine, TLineSerial> request,
        CancellationToken cancellationToken = default)
        where TLine : BaseLineEntity
        where TLineSerial : BaseLineSerialEntity;
}
