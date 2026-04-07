using Wms.Infrastructure.Services.Common;
using Wms.Application.Common;
using Wms.Application.GoodsReceipt.Dtos;
using Wms.Application.GoodsReceipt.Services;
using Wms.Application.WarehouseInbound.Dtos;
using Wms.Application.WarehouseInbound.Services;
using Wms.Application.WarehouseOutbound.Dtos;
using Wms.Application.WarehouseOutbound.Services;
using Wms.Application.WarehouseTransfer.Dtos;
using Wms.Application.WarehouseTransfer.Services;
using Wms.Domain.Entities.WarehouseTransfer;

var routes = new[]
{
    new ProcessWoRouteDto
    {
        StockId = 10,
        StockCode = "STK-001",
        YapKodId = 100,
        YapKod = "YP-A",
        Quantity = 2
    },
    new ProcessWoRouteDto
    {
        StockId = 10,
        StockCode = "STK-001",
        YapKodId = 100,
        YapKod = "YP-A",
        Quantity = 3
    },
    new ProcessWoRouteDto
    {
        StockId = 11,
        StockCode = "STK-002",
        YapKodId = 101,
        YapKod = "YP-B",
        Quantity = 1
    }
};

var seeds = WoProcessGroupingHelper.BuildImportLineSeeds(routes);

if (seeds.Count != 2)
{
    throw new Exception($"Expected 2 import line groups, got {seeds.Count}.");
}

if (seeds.Any(x => string.IsNullOrWhiteSpace(x.StockCode)))
{
    throw new Exception("Grouped import line seed contains empty stock code.");
}

Console.WriteLine("WO process smoke test passed.");
Console.WriteLine($"ImportLine group count: {seeds.Count}");
foreach (var seed in seeds)
{
    Console.WriteLine($"{seed.GroupingKey} => StockId:{seed.StockId}, StockCode:{seed.StockCode}, YapKodId:{seed.YapKodId}, YapKod:{seed.YapKod}");
}

var grRoutes = new[]
{
    new ProcessGrRouteDto
    {
        StockId = 20,
        StockCode = "GR-001",
        YapKodId = 200,
        YapKod = "CFG-A",
        Quantity = 5
    },
    new ProcessGrRouteDto
    {
        StockId = 20,
        StockCode = "GR-001",
        YapKodId = 200,
        YapKod = "CFG-A",
        Quantity = 1
    },
    new ProcessGrRouteDto
    {
        StockId = 21,
        StockCode = "GR-002",
        YapKodId = 201,
        YapKod = "CFG-B",
        Quantity = 2
    }
};

var grSeeds = GrProcessGroupingHelper.BuildImportLineSeeds(grRoutes);
if (grSeeds.Count != 2)
{
    throw new Exception($"Expected 2 goods receipt import line groups, got {grSeeds.Count}.");
}

Console.WriteLine("GR process smoke test passed.");
Console.WriteLine($"ImportLine group count: {grSeeds.Count}");
foreach (var seed in grSeeds)
{
    Console.WriteLine($"{seed.GroupingKey} => StockId:{seed.StockId}, StockCode:{seed.StockCode}, YapKodId:{seed.YapKodId}, YapKod:{seed.YapKod}");
}

var wiRoutes = new[]
{
    new ProcessWiRouteDto
    {
        StockId = 30,
        StockCode = "WI-001",
        YapKodId = 300,
        YapKod = "CFG-X",
        Quantity = 4
    },
    new ProcessWiRouteDto
    {
        StockId = 30,
        StockCode = "WI-001",
        YapKodId = 300,
        YapKod = "CFG-X",
        Quantity = 2
    },
    new ProcessWiRouteDto
    {
        StockId = 31,
        StockCode = "WI-002",
        YapKodId = 301,
        YapKod = "CFG-Y",
        Quantity = 1
    }
};

var wiSeeds = WiProcessGroupingHelper.BuildImportLineSeeds(wiRoutes);
if (wiSeeds.Count != 2)
{
    throw new Exception($"Expected 2 warehouse inbound import line groups, got {wiSeeds.Count}.");
}

Console.WriteLine("WI process smoke test passed.");
Console.WriteLine($"ImportLine group count: {wiSeeds.Count}");
foreach (var seed in wiSeeds)
{
    Console.WriteLine($"{seed.GroupingKey} => StockId:{seed.StockId}, StockCode:{seed.StockCode}, YapKodId:{seed.YapKodId}, YapKod:{seed.YapKod}");
}

var wtRoutes = new[]
{
    new ProcessWtRouteDto
    {
        StockId = 40,
        StockCode = "WT-001",
        YapKodId = 400,
        YapKod = "CFG-T1",
        Quantity = 6
    },
    new ProcessWtRouteDto
    {
        StockId = 40,
        StockCode = "WT-001",
        YapKodId = 400,
        YapKod = "CFG-T1",
        Quantity = 2
    },
    new ProcessWtRouteDto
    {
        StockId = 41,
        StockCode = "WT-002",
        YapKodId = 401,
        YapKod = "CFG-T2",
        Quantity = 1
    }
};

var wtSeeds = WtProcessGroupingHelper.BuildImportLineSeeds(wtRoutes);
if (wtSeeds.Count != 2)
{
    throw new Exception($"Expected 2 warehouse transfer import line groups, got {wtSeeds.Count}.");
}

Console.WriteLine("WT process smoke test passed.");
Console.WriteLine($"ImportLine group count: {wtSeeds.Count}");
foreach (var seed in wtSeeds)
{
    Console.WriteLine($"{seed.GroupingKey} => StockId:{seed.StockId}, StockCode:{seed.StockCode}, YapKodId:{seed.YapKodId}, YapKod:{seed.YapKod}");
}


var parser = new BarcodeParser();
var parsed = parser.Parse("STK-001///YP-A///SR-9001", "StockCode///YapKod///SerialNumber");
if (!parsed.Success)
{
    throw new Exception("Expected barcode parser to parse the configured format.");
}

if (parsed.StockCode != "STK-001" || parsed.YapKod != "YP-A" || parsed.SerialNumber != "SR-9001")
{
    throw new Exception($"Unexpected barcode parser result: Stock={parsed.StockCode}, YapKod={parsed.YapKod}, Serial={parsed.SerialNumber}");
}

Console.WriteLine("Barcode parser smoke test passed.");
Console.WriteLine($"Parsed => StockCode:{parsed.StockCode}, YapKod:{parsed.YapKod}, Serial:{parsed.SerialNumber}");

var matchingService = new AssignedBarcodeMatchingService(new FakeBarcodeResolutionService());
var lineA = new WtLine { Id = 1001, HeaderId = 1, StockCode = "WT-001", YapKod = "CFG-T1" };
var lineB = new WtLine { Id = 1002, HeaderId = 1, StockCode = "WT-001", YapKod = "CFG-T1" };
var serials = new[]
{
    new WtLineSerial { Id = 1, LineId = 1001, Quantity = 5, SerialNo = "SR-1" },
    new WtLineSerial { Id = 2, LineId = 1002, Quantity = 7, SerialNo = "SR-2" }
};

var match = await matchingService.MatchAsync(new AssignedBarcodeMatchRequest<WtLine, WtLineSerial>
{
    BarcodeRequest = new ResolveBarcodeRequestDto
    {
        ModuleKey = BarcodeModuleKeys.WarehouseTransferAssigned,
        Barcode = "WT-001///CFG-T1///SR-2",
        FallbackStockCode = "WT-001",
        FallbackYapKod = "CFG-T1",
        FallbackSerialNumber = "SR-2"
    },
    RequestQuantity = 1,
    RawBarcode = "WT-001///CFG-T1///SR-2",
    AllowMoreQuantityBasedOnOrder = false,
    Lines = new[] { lineA, lineB },
    LineSerials = serials,
    ExistingRoutes = Array.Empty<AssignedBarcodeRouteSnapshot>(),
    LineIdSelector = line => line.Id,
    LineSerialLineIdSelector = serial => serial.LineId,
    StockAndYapKodNotMatchedErrorCode = "NO_STOCK_MATCH",
    SerialNotMatchedErrorCode = "NO_SERIAL_MATCH",
    NoMatchingLineErrorCode = "NO_LINE_MATCH",
    QuantityExceededErrorCode = "OVER_QTY"
});

if (!match.Success || match.SelectedLineId != 1002)
{
    throw new Exception($"Expected assigned matcher to select line 1002, got {match.SelectedLineId}.");
}

var duplicate = await matchingService.MatchAsync(new AssignedBarcodeMatchRequest<WtLine, WtLineSerial>
{
    BarcodeRequest = new ResolveBarcodeRequestDto
    {
        ModuleKey = BarcodeModuleKeys.WarehouseTransferAssigned,
        Barcode = "WT-001///CFG-T1///SR-2",
        FallbackStockCode = "WT-001",
        FallbackYapKod = "CFG-T1",
        FallbackSerialNumber = "SR-2"
    },
    RequestQuantity = 1,
    RawBarcode = "WT-001///CFG-T1///SR-2",
    SourceCellCode = "A-01",
    TargetCellCode = "B-01",
    AllowMoreQuantityBasedOnOrder = false,
    Lines = new[] { lineA, lineB },
    LineSerials = serials,
    ExistingRoutes = new[]
    {
        new AssignedBarcodeRouteSnapshot
        {
            LineId = 1002,
            ScannedBarcode = "WT-001///CFG-T1///SR-2",
            SerialNo = "SR-2",
            Quantity = 1,
            SourceCellCode = "A-01",
            TargetCellCode = "B-01"
        }
    },
    LineIdSelector = line => line.Id,
    LineSerialLineIdSelector = serial => serial.LineId,
    StockAndYapKodNotMatchedErrorCode = "NO_STOCK_MATCH",
    SerialNotMatchedErrorCode = "NO_SERIAL_MATCH",
    NoMatchingLineErrorCode = "NO_LINE_MATCH",
    QuantityExceededErrorCode = "OVER_QTY"
});

if (duplicate.Success || duplicate.ErrorCode != "BarcodeAlreadyScanned")
{
    throw new Exception("Expected assigned matcher duplicate scan guard to reject repeated barcode.");
}

Console.WriteLine("Assigned barcode matching smoke test passed.");

file sealed class FakeBarcodeResolutionService : IBarcodeResolutionService
{
    public Task<ResolvedBarcodeDto> ResolveAsync(ResolveBarcodeRequestDto request, CancellationToken cancellationToken = default)
    {
        var parts = request.Barcode.Split("///", StringSplitOptions.None);
        return Task.FromResult(new ResolvedBarcodeDto
        {
            ModuleKey = request.ModuleKey,
            Barcode = request.Barcode,
            StockCode = parts.ElementAtOrDefault(0) ?? request.FallbackStockCode,
            YapKod = parts.ElementAtOrDefault(1) ?? request.FallbackYapKod,
            SerialNumber = parts.ElementAtOrDefault(2) ?? request.FallbackSerialNumber,
            ReasonCode = BarcodeMatchReasonCode.ParsedByDefinition
        });
    }
}
