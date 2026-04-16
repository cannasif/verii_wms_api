namespace Wms.Domain.Entities.ServiceAllocation.Enums;

public enum AllocationStatus
{
    Waiting = 0,
    PartiallyAllocated = 1,
    Allocated = 2,
    Reserved = 3,
    PartiallyShipped = 4,
    Shipped = 5,
    Blocked = 6,
    Cancelled = 7
}
