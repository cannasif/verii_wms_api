using Microsoft.Data.SqlClient;
using Wms.Application.YapKod.Services;
using Wms.Domain.Entities.YapKod.Functions;

namespace Wms.Infrastructure.Services.YapKod;

public sealed class YapKodErpReadService : IYapKodErpReadService
{
    private readonly IConfiguration _configuration;

    public YapKodErpReadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<FnYapKodRow>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString("ErpConnection");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("ErpConnection is not configured.");
        }

        var result = new List<FnYapKodRow>();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM dbo.RII_FN_ESNYAPMAS()";

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            result.Add(new FnYapKodRow
            {
                YapKod = GetNullableString(reader, "YAPKOD") ?? string.Empty,
                YapAcik = GetNullableString(reader, "YAPACIK") ?? string.Empty,
                SubeKodu = GetNullableValue<short>(reader, "SUBE_KODU"),
                YplndrStokKod = GetNullableString(reader, "YPLNDRSTOKKOD")
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
