using Microsoft.Data.SqlClient;
using Wms.Application.Customer.Services;
using Wms.Domain.Entities.Customer.Functions;

namespace Wms.Infrastructure.Services.Customer;

public sealed class CustomerErpReadService : ICustomerErpReadService
{
    private readonly IConfiguration _configuration;

    public CustomerErpReadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<FnCustomerRow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("ErpConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ErpConnection is not configured.");
        }

        var result = new List<FnCustomerRow>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM dbo.RII_FN_CARI(@cariKodu, @branchCode)";
        command.Parameters.AddWithValue("@cariKodu", DBNull.Value);
        command.Parameters.AddWithValue("@branchCode", DBNull.Value);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new FnCustomerRow
            {
                SubeKodu = GetNullableValue<short>(reader, "SUBE_KODU"),
                IsletmeKodu = GetNullableValue<short>(reader, "ISLETME_KODU"),
                CariKod = GetNullableString(reader, "CARI_KOD") ?? string.Empty,
                CariIsim = GetNullableString(reader, "CARI_ISIM")
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
