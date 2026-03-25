namespace WMS_WEBAPI.Enums;

/// <summary>
/// ERP entegrasyon durumunu temsil eder.
/// </summary>
public enum ErpIntegrationStatusType
{
    /// <summary>
    /// Entegrasyon henüz başlamadı.
    /// </summary>
    NotStarted = 0,

    /// <summary>
    /// Entegrasyon başarılı oldu.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Entegrasyon başarısız oldu.
    /// </summary>
    Failed = 2,

    /// <summary>
    /// Entegrasyon tekrar denemede.
    /// </summary>
    Retrying = 3
}

