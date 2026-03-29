using Wms.Application.Common;

namespace Wms.Application.Definitions.Dtos.Parameters;

/// <summary>
/// Parameter modüllerinin ortak DTO alanlarını tutar.
/// </summary>
public class BaseParameterDto : BaseEntityDto
{
    public bool AllowLessQuantityBasedOnOrder { get; set; }
    public bool AllowMoreQuantityBasedOnOrder { get; set; }
    public bool RequireApprovalBeforeErp { get; set; }
    public bool RequireAllOrderItemsCollected { get; set; }
}

public class CreateBaseParameterDto
{
    public bool AllowLessQuantityBasedOnOrder { get; set; }
    public bool AllowMoreQuantityBasedOnOrder { get; set; }
    public bool RequireApprovalBeforeErp { get; set; }
    public bool RequireAllOrderItemsCollected { get; set; }
}

public class UpdateBaseParameterDto
{
    public bool? AllowLessQuantityBasedOnOrder { get; set; }
    public bool? AllowMoreQuantityBasedOnOrder { get; set; }
    public bool? RequireApprovalBeforeErp { get; set; }
    public bool? RequireAllOrderItemsCollected { get; set; }
}
