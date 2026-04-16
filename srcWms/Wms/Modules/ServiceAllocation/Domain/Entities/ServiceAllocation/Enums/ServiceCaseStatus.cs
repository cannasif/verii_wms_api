namespace Wms.Domain.Entities.ServiceAllocation.Enums;

public enum ServiceCaseStatus
{
    Draft = 0,
    WaitingForIntake = 1,
    Received = 2,
    InDiagnosis = 3,
    WaitingForParts = 4,
    InRepair = 5,
    ReadyForReturn = 6,
    ReturnedToMainWarehouse = 7,
    Closed = 8,
    Cancelled = 9
}
