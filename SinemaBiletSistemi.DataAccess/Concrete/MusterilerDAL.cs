using System.Data;
using Microsoft.Data.SqlClient;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Concrete;

public sealed class MusterilerDAL : IMusterilerDAL
{
    private readonly IConnectionFactory _connectionFactory;

    public MusterilerDAL(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Musteri>> MusteriListeleAsync(CancellationToken cancellationToken = default)
    {
        var musteriler = new List<Musteri>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_MusteriListele", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            musteriler.Add(new Musteri
            {
                MusteriID = GetInt32(reader, "MusteriID"),
                AdSoyad = GetString(reader, "AdSoyad"),
                Telefon = GetString(reader, "Telefon"),
                Email = GetString(reader, "Email")
            });
        }

        return musteriler;
    }

    public async Task MusteriEkleAsync(Musteri musteri, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_MusteriEkle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        AddMusteriParameters(command, musteri, includeId: false);

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task MusteriGuncelleAsync(Musteri musteri, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_MusteriGuncelle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        AddMusteriParameters(command, musteri, includeId: true);

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task MusteriSilAsync(int musteriID, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_MusteriSil", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@MusteriID", SqlDbType.Int).Value = musteriID;

        await connection.OpenAsync(cancellationToken);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static void AddMusteriParameters(SqlCommand command, Musteri musteri, bool includeId)
    {
        if (includeId)
        {
            command.Parameters.Add("@MusteriID", SqlDbType.Int).Value = musteri.MusteriID;
        }

        command.Parameters.Add("@AdSoyad", SqlDbType.NVarChar).Value = musteri.AdSoyad;
        command.Parameters.Add("@Telefon", SqlDbType.NVarChar).Value = musteri.Telefon;
        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = musteri.Email;
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
