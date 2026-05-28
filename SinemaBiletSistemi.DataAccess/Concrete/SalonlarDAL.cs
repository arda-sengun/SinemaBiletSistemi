using System.Data;
using Microsoft.Data.SqlClient;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Concrete;

public sealed class SalonlarDAL : ISalonlarDAL
{
    private readonly IConnectionFactory _connectionFactory;

    public SalonlarDAL(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Salon>> SalonListeleAsync(CancellationToken cancellationToken = default)
    {
        var salonlar = new List<Salon>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_SalonListele", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            salonlar.Add(new Salon
            {
                SalonID = GetInt32(reader, "SalonID"),
                SalonAdi = GetString(reader, "SalonAdi"),
                Kapasite = GetInt32(reader, "Kapasite")
            });
        }

        return salonlar;
    }

    public async Task SalonEkleAsync(Salon salon, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_SalonEkle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@SalonAdi", SqlDbType.NVarChar).Value = salon.SalonAdi;
        command.Parameters.Add("@Kapasite", SqlDbType.Int).Value = salon.Kapasite;

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
