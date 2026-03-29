using Wms.Domain.Entities.Common;

namespace Wms.Domain.Entities.Definitions;

/// <summary>
/// `_old/reference/verii_wms_api.Domain/Entities/Common/BaseParameter.cs` ortak parameter alanlarını korur.
/// </summary>
public abstract class BaseParameter : BaseEntity
{
    public bool AllowLessQuantityBasedOnOrder { get; set; }
    public bool AllowMoreQuantityBasedOnOrder { get; set; }
    public bool RequireApprovalBeforeErp { get; set; }
    public bool RequireAllOrderItemsCollected { get; set; }
}
