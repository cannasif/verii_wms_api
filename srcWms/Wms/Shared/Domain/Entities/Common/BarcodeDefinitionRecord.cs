namespace Wms.Domain.Entities.Common;

public sealed class BarcodeDefinitionRecord : BaseEntity
{
    public string ModuleKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DefinitionType { get; set; } = "pattern";
    public string SegmentPattern { get; set; } = string.Empty;
    public string RequiredSegments { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool AllowMirrorLookup { get; set; } = true;
    public string HintText { get; set; } = string.Empty;
}
