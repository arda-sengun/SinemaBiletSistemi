using System.Data;
using Microsoft.Data.SqlClient;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Concrete;

public sealed class SeanslarDAL : ISeanslarDAL
{
    private readonly IConnectionFactory _connectionFactory;

    public SeanslarDAL(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Seans>> SeansListeleAsync(CancellationToken cancellationToken = default)
    {
        var seanslar = new List<Seans>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_SeansListele", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            seanslar.Add(new Seans
            {
                SeansID = GetInt32(reader, "SeansID"),
                FilmID = GetInt32(reader, "FilmID"),
                SalonID = GetInt32(reader, "SalonID"),
                TarihSaat = GetDateTime(reader, "TarihSaat"),
                BiletFiyati = GetDecimal(reader, "BiletFiyati"),
                BosKoltukSayisi = GetInt32(reader, "BosKoltukSayisi")
            });
        }

        return seanslar;
    }

    public async Task SeansEkleAsync(Seans seans, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_SeansEkle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@FilmID", SqlDbType.Int).Value = seans.FilmID;
        command.Parameters.Add("@SalonID", SqlDbType.Int).Value = seans.SalonID;
        command.Parameters.Add("@TarihSaat", SqlDbType.DateTime).Value = seans.TarihSaat;
        command.Parameters.Add("@BiletFiyati", SqlDbType.Decimal).Value = seans.BiletFiyati;

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task SeansGuncelleAsync(Seans seans, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_SeansGuncelle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        AddSeansParameters(command, seans, includeId: true);

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task SeansSilAsync(int seansID, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_SeansSil", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@SeansID", SqlDbType.Int).Value = seansID;

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static void AddSeansParameters(SqlCommand command, Seans seans, bool includeId)
    {
        if (includeId)
        {
            command.Parameters.Add("@SeansID", SqlDbType.Int).Value = seans.SeansID;
        }

        command.Parameters.Add("@FilmID", SqlDbType.Int).Value = seans.FilmID;
        command.Parameters.Add("@SalonID", SqlDbType.Int).Value = seans.SalonID;
        command.Parameters.Add("@TarihSaat", SqlDbType.DateTime).Value = seans.TarihSaat;
        command.Parameters.Add("@BiletFiyati", SqlDbType.Decimal).Value = seans.BiletFiyati;
        command.Parameters.Add("@BosKoltukSayisi", SqlDbType.Int).Value = seans.BosKoltukSayisi;
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
