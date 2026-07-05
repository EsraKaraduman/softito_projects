using System.Data;
using Microsoft.Data.SqlClient;

namespace HotelProject.API.Data
{
    public class DbConnectionProvider
    {
        private readonly string _connectionString;

        public DbConnectionProvider(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("DefaultConnection connection string not found.");
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public SqlConnection CreateSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
