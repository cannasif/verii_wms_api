using System.ComponentModel.DataAnnotations;
using Wms.Application.Common;

namespace Wms.Application.Production.Dtos;

public sealed class PrHeaderSerialDto : BaseEntityDto
{
    public long HeaderId { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public decimal? Amount { get; set; }
}

public sealed class CreatePrHeaderSerialDto
{
    [Required] public long HeaderId { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public decimal? Amount { get; set; }
}

public sealed class UpdatePrHeaderSerialDto
{
    public long? HeaderId { get; set; }
    public string? SerialNo { get; set; }
    public string? SerialNo2 { get; set; }
    public string? SerialNo3 { get; set; }
    public string? SerialNo4 { get; set; }
    public decimal? Amount { get; set; }
}
