using System.Data;
using Microsoft.Data.SqlClient;
using SinemaBiletSistemi.DataAccess.Abstract;
using SinemaBiletSistemi.Entities;

namespace SinemaBiletSistemi.DataAccess.Concrete;

public sealed class FilmlerDAL : IFilmlerDAL
{
    private readonly IConnectionFactory _connectionFactory;

    public FilmlerDAL(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Film>> FilmListeleAsync(CancellationToken cancellationToken = default)
    {
        var filmler = new List<Film>();

        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_FilmListele", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        await connection.OpenAsync(cancellationToken);
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            filmler.Add(new Film
            {
                FilmID = GetInt32(reader, "FilmID"),
                FilmAdi = GetString(reader, "FilmAdi"),
                Yonetmen = GetString(reader, "Yonetmen"),
                Sure = GetInt32(reader, "Sure"),
                Tur = GetString(reader, "Tur"),
                AfisURL = GetString(reader, "AfisURL")
            });
        }

        return filmler;
    }

    public async Task FilmEkleAsync(Film film, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.CreateConnection();
        await using var command = new SqlCommand("sp_FilmEkle", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        command.Parameters.Add("@FilmAdi", SqlDbType.NVarChar).Value = film.FilmAdi;
        command.Parameters.Add("@Yonetmen", SqlDbType.NVarChar).Value = film.Yonetmen;
        command.Parameters.Add("@Sure", SqlDbType.Int).Value = film.Sure;
        command.Parameters.Add("@Tur", SqlDbType.NVarChar).Value = film.Tur;
        command.Parameters.Add("@AfisURL", SqlDbType.NVarChar).Value = film.AfisURL;

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
