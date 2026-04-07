namespace Wms.WebApi.Options;

public sealed class BarcodeDefinitionsOptions
{
    public bool AllowMirrorLookup { get; set; } = true;
    public List<BarcodeModuleDefinitionOptions> Modules { get; set; } = new();
}

public sealed class BarcodeModuleDefinitionOptions
{
    public string ModuleKey { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string DefinitionType { get; set; } = "pattern";
    public string SegmentPattern { get; set; } = string.Empty;
    public string RequiredSegments { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool AllowMirrorLookup { get; set; } = true;
    public string? HintText { get; set; }

    public string Format
    {
        get => SegmentPattern;
        set => SegmentPattern = value;
    }

    public bool EnableErpFallback
    {
        get => AllowMirrorLookup;
        set => AllowMirrorLookup = value;
    }
}
