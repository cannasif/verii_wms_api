namespace WMS_WEBAPI.DTOs
{
    public class BaseLineSerialUpdateDto
    {
        public decimal? Quantity { get; set; }
        public string? SerialNo { get; set; }
        public string? SerialNo2 { get; set; }
        public string? SerialNo3 { get; set; }
        public string? SerialNo4 { get; set; }
        public string? SourceCellCode { get; set; }
        public string? TargetCellCode { get; set; }
    }
}