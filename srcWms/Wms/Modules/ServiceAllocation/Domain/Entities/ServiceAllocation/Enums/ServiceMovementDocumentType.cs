namespace Wms.Domain.Entities.ServiceAllocation.Enums;

public enum ServiceMovementDocumentType
{
    WarehouseInbound = 0,
    WarehouseTransfer = 1,
    WarehouseOutbound = 2,
    Shipping = 3,
    SubcontractingIssue = 4,
    SubcontractingReceipt = 5
}
