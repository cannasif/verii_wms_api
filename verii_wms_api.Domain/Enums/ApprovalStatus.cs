namespace WMS_WEBAPI.Enums;

/// <summary>
/// Onay durumunu temsil eder.
/// </summary>
public enum ApprovalStatusType
{
    /// <summary>
    /// Onay bekleniyor.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Onaylandı.
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Reddedildi.
    /// </summary>
    Rejected = 2
}

