using Wms.Domain.Entities.ServiceAllocation.Enums;

namespace Wms.Application.ServiceAllocation.Services;

public interface IOperationAllocationOrchestrator
{
    Task TriggerForDocumentAsync(DocumentModule documentModule, long documentHeaderId, CancellationToken cancellationToken = default);
}
