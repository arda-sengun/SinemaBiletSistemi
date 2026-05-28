using System.Data;
using Microsoft.Data.SqlClient;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Concrete;

public sealed class BiletlerDAL : IBiletlerDAL
{
    private readonly IConnectionFactory _connectionFactory;

    public BiletlerDAL(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Bilet>> BiletListeleAsync(CancellationToken cancellationToken = default)
    {
        var biletler = new List<Bilet>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_BiletListele", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            biletler.Add(new Bilet
            {
                BiletID = GetInt32(reader, "BiletID"),
                SeansID = GetInt32(reader, "SeansID"),
                MusteriID = GetInt32(reader, "MusteriID"),
                KoltukID = GetInt32(reader, "KoltukID"),
                SatisTarihi = GetDateTime(reader, "SatisTarihi"),
                OdemeTutari = GetDecimal(reader, "OdemeTutari")
            });
        }

        return biletler;
    }

    public async Task BiletEkleAsync(Bilet bilet, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_BiletEkle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@SeansID", SqlDbType.Int).Value = bilet.SeansID;
        command.Parameters.Add("@MusteriID", SqlDbType.Int).Value = bilet.MusteriID;
        command.Parameters.Add("@KoltukID", SqlDbType.Int).Value = bilet.KoltukID;

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task BiletGuncelleAsync(Bilet bilet, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_BiletGuncelle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        AddBiletParameters(command, bilet, includeId: true);

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task BiletSilAsync(int biletID, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_BiletSil", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@BiletID", SqlDbType.Int).Value = biletID;

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static void AddBiletParameters(SqlCommand command, Bilet bilet, bool includeId)
    {
        if (includeId)
        {
            command.Parameters.Add("@BiletID", SqlDbType.Int).Value = bilet.BiletID;
        }

        command.Parameters.Add("@SeansID", SqlDbType.Int).Value = bilet.SeansID;
        command.Parameters.Add("@MusteriID", SqlDbType.Int).Value = bilet.MusteriID;
        command.Parameters.Add("@KoltukID", SqlDbType.Int).Value = bilet.KoltukID;
        command.Parameters.Add("@SatisTarihi", SqlDbType.DateTime).Value = bilet.SatisTarihi;
        command.Parameters.Add("@OdemeTutari", SqlDbType.Decimal).Value = bilet.OdemeTutari;
    }

    private static int GetInt32(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
    }

    private static DateTime GetDateTime(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? default : reader.GetDateTime(ordinal);
    }

    private static decimal GetDecimal(SqlDataReader reader, string columnName)
    {
        var ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? 0 : reader.GetDecimal(ordinal);
    }
}
