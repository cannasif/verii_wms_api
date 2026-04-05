using Microsoft.Data.SqlClient;
using Wms.Application.Stock.Services;
using Wms.Domain.Entities.Stock.Functions;

namespace Wms.Infrastructure.Services.Stock;

public sealed class StockErpReadService : IStockErpReadService
{
    private readonly IConfiguration _configuration;

    public StockErpReadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<FnStockRow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("ErpConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ErpConnection is not configured.");
        }

        var result = new List<FnStockRow>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM dbo.RII_FN_STOK(@stokKodu, @branchCode)";
        command.Parameters.AddWithValue("@stokKodu", DBNull.Value);
        command.Parameters.AddWithValue("@branchCode", DBNull.Value);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new FnStockRow
            {
                SubeKodu = GetNullableValue<short>(reader, "SUBE_KODU"),
                IsletmeKodu = GetNullableValue<short>(reader, "ISLETME_KODU"),
                StokKodu = GetNullableString(reader, "STOK_KODU") ?? string.Empty,
                UreticiKodu = GetNullableString(reader, "URETICI_KODU"),
                StokAdi = GetNullableString(reader, "STOK_ADI"),
                GrupKodu = GetNullableString(reader, "GRUP_KODU"),
                Kod1 = GetNullableString(reader, "KOD_1"),
                Kod2 = GetNullableString(reader, "KOD_2"),
                Kod3 = GetNullableString(reader, "KOD_3"),
                Kod4 = GetNullableString(reader, "KOD_4"),
                Kod5 = GetNullableString(reader, "KOD_5")
            });
        }

        return result;
    }

    private static string? GetNullableString(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
    }

    private static T? GetNullableValue<T>(SqlDataReader reader, string columnName) where T : struct
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? null : reader.GetFieldValue<T>(ordinal);
    }
}
