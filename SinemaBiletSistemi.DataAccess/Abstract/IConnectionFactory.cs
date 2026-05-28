using Microsoft.Data.SqlClient;

namespace SinemaBiletSistemi.DataAccess.Abstract;

public interface IConnectionFactory
{
    SqlConnection CreateConnection();
}
