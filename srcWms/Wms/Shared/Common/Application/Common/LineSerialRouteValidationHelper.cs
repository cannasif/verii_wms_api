using Wms.Domain.Entities.Common;

namespace Wms.Application.Common;

public static class LineSerialRouteValidationHelper
{
    private const decimal Tolerance = 0.000001m;

    public static LineSerialRouteValidationResult Validate(
        IReadOnlyCollection<BaseLineSerialEntity> lineSerials,
        IReadOnlyCollection<BaseRouteEntity> routes,
        bool allowLess,
        bool allowMore)
    {
        var totalLineSerialQuantity = lineSerials.Sum(x => x.Quantity);
        var totalRouteQuantity = routes.Sum(x => x.Quantity);

        var hasDetail =
            lineSerials.Any(HasDetail) ||
            routes.Any(HasDetail);

        if (!hasDetail)
        {
            return ValidatePair(totalLineSerialQuantity, totalRouteQuantity, allowLess, allowMore);
        }

        var lineSerialTotals = lineSerials
            .GroupBy(BuildLineSerialKey)
            .ToDictionary(group => group.Key, group => group.Sum(item => item.Quantity), StringComparer.OrdinalIgnoreCase);

        var routeTotals = routes
            .GroupBy(BuildRouteKey)
            .ToDictionary(group => group.Key, group => group.Sum(item => item.Quantity), StringComparer.OrdinalIgnoreCase);

        var allKeys = lineSerialTotals.Keys
            .Union(routeTotals.Keys, StringComparer.OrdinalIgnoreCase)
            .ToArray();

        foreach (var key in allKeys)
        {
            var expectedQuantity = lineSerialTotals.TryGetValue(key, out var expected) ? expected : 0m;
            var actualQuantity = routeTotals.TryGetValue(key, out var actual) ? actual : 0m;

            var result = ValidatePair(expectedQuantity, actualQuantity, allowLess, allowMore);
            if (result.HasMismatch)
            {
                return result;
            }
        }

        return LineSerialRouteValidationResult.Success(totalLineSerialQuantity, totalRouteQuantity);
    }

    private static bool HasDetail(BaseLineSerialEntity entity)
        => !string.IsNullOrWhiteSpace(entity.SerialNo)
           || !string.IsNullOrWhiteSpace(entity.SerialNo2)
           || !string.IsNullOrWhiteSpace(entity.SerialNo3)
           || !string.IsNullOrWhiteSpace(entity.SerialNo4)
           || entity.SourceWarehouseId.HasValue
           || entity.TargetWarehouseId.HasValue
           || !string.IsNullOrWhiteSpace(entity.SourceCellCode)
           || !string.IsNullOrWhiteSpace(entity.TargetCellCode);

    private static bool HasDetail(BaseRouteEntity entity)
        => !string.IsNullOrWhiteSpace(entity.SerialNo)
           || !string.IsNullOrWhiteSpace(entity.SerialNo2)
           || !string.IsNullOrWhiteSpace(entity.SerialNo3)
           || !string.IsNullOrWhiteSpace(entity.SerialNo4)
           || entity.SourceWarehouse.HasValue
           || entity.TargetWarehouse.HasValue
           || !string.IsNullOrWhiteSpace(entity.SourceCellCode)
           || !string.IsNullOrWhiteSpace(entity.TargetCellCode);

    private static string BuildLineSerialKey(BaseLineSerialEntity entity)
        => string.Join("|",
            Normalize(entity.SerialNo),
            Normalize(entity.SerialNo2),
            Normalize(entity.SerialNo3),
            Normalize(entity.SerialNo4),
            Normalize(entity.SourceWarehouseId),
            Normalize(entity.TargetWarehouseId),
            Normalize(entity.SourceCellCode),
            Normalize(entity.TargetCellCode));

    private static string BuildRouteKey(BaseRouteEntity entity)
        => string.Join("|",
            Normalize(entity.SerialNo),
            Normalize(entity.SerialNo2),
            Normalize(entity.SerialNo3),
            Normalize(entity.SerialNo4),
            Normalize(entity.SourceWarehouse),
            Normalize(entity.TargetWarehouse),
            Normalize(entity.SourceCellCode),
            Normalize(entity.TargetCellCode));

    private static LineSerialRouteValidationResult ValidatePair(
        decimal expectedQuantity,
        decimal actualQuantity,
        bool allowLess,
        bool allowMore)
    {
        if (!allowLess && !allowMore && Math.Abs(expectedQuantity - actualQuantity) > Tolerance)
        {
            return LineSerialRouteValidationResult.Mismatch(expectedQuantity, actualQuantity, LineSerialRouteValidationMismatch.ExactMatchRequired);
        }

        if (allowLess && !allowMore && actualQuantity > expectedQuantity + Tolerance)
        {
            return LineSerialRouteValidationResult.Mismatch(expectedQuantity, actualQuantity, LineSerialRouteValidationMismatch.CannotBeGreater);
        }

        if (!allowLess && allowMore && actualQuantity + Tolerance < expectedQuantity)
        {
            return LineSerialRouteValidationResult.Mismatch(expectedQuantity, actualQuantity, LineSerialRouteValidationMismatch.CannotBeLess);
        }

        return LineSerialRouteValidationResult.Success(expectedQuantity, actualQuantity);
    }

    private static string Normalize(string? value)
        => (value ?? string.Empty).Trim().ToUpperInvariant();

    private static string Normalize(long? value)
        => value?.ToString() ?? string.Empty;

    private static string Normalize(int? value)
        => value?.ToString() ?? string.Empty;
}

public enum LineSerialRouteValidationMismatch
{
    None = 0,
    ExactMatchRequired = 1,
    CannotBeGreater = 2,
    CannotBeLess = 3
}

public sealed record LineSerialRouteValidationResult(
    bool HasMismatch,
    decimal ExpectedQuantity,
    decimal ActualQuantity,
    LineSerialRouteValidationMismatch MismatchType)
{
    public static LineSerialRouteValidationResult Success(decimal expectedQuantity, decimal actualQuantity)
        => new(false, expectedQuantity, actualQuantity, LineSerialRouteValidationMismatch.None);

    public static LineSerialRouteValidationResult Mismatch(
        decimal expectedQuantity,
        decimal actualQuantity,
        LineSerialRouteValidationMismatch mismatchType)
        => new(true, expectedQuantity, actualQuantity, mismatchType);
}
