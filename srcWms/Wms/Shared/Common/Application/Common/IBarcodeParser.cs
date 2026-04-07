namespace Wms.Application.Common;

public interface IBarcodeParser
{
    BarcodeParseResultDto Parse(string rawBarcode, string format);
}
