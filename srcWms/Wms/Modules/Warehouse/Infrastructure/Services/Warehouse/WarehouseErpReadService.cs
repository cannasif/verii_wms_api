using Microsoft.Data.SqlClient;
using Wms.Application.Warehouse.Services;
using Wms.Domain.Entities.Warehouse.Functions;

namespace Wms.Infrastructure.Services.Warehouse;

public sealed class WarehouseErpReadService : IWarehouseErpReadService
{
    private readonly IConfiguration _configuration;

    public WarehouseErpReadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<FnWarehouseRow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("ErpConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ErpConnection is not configured.");
        }

        var result = new List<FnWarehouseRow>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM dbo.RII_FN_DEPO(@depoKodu, @branchCode)";
        command.Parameters.AddWithValue("@depoKodu", DBNull.Value);
        command.Parameters.AddWithValue("@branchCode", DBNull.Value);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new FnWarehouseRow
            {
                DepoKodu = GetNullableValue<short>(reader, "DEPO_KODU"),
                DepoIsmi = GetNullableString(reader, "DEPO_ISMI")
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
