using System.Data;
using Microsoft.Data.SqlClient;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Concrete;

public sealed class KoltuklarDAL : IKoltuklarDAL
{
    private readonly IConnectionFactory _connectionFactory;

    public KoltuklarDAL(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Koltuk>> KoltukListeleAsync(CancellationToken cancellationToken = default)
    {
        var koltuklar = new List<Koltuk>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_KoltukListele", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            koltuklar.Add(new Koltuk
            {
                KoltukID = GetInt32(reader, "KoltukID"),
                SalonID = GetInt32(reader, "SalonID"),
                KoltukNo = GetString(reader, "KoltukNo")
            });
        }

        return koltuklar;
    }

    public async Task KoltukEkleAsync(Koltuk koltuk, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_KoltukEkle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@SalonID", SqlDbType.Int).Value = koltuk.SalonID;
        command.Parameters.Add("@KoltukNo", SqlDbType.NVarChar).Value = koltuk.KoltukNo;

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static int GetInt32(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
    }

    private static string GetString(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
    }
}
