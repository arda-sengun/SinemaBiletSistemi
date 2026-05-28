using Microsoft.Data.SqlClient;
using SinemaBiletSistemi.DataAccess.Abstract;

namespace SinemaBiletSistemi.DataAccess.Concrete;

public sealed class SqlConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Veritabani baglanti cumlesi bos olamaz.", nameof(connectionString));
        }

        _connectionString = connectionString;
    }

    public SqlConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
