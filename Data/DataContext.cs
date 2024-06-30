using Microsoft.Data.SqlClient;
using System.Data;

namespace RandomApp1.Data
{
    public class DataContext
    {
        private readonly string _connectionString;

        public DataContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Sql");
        }

        public IDbConnection createConnection()
        {
            var connection = new SqlConnection(_connectionString);
            if (connection is null) throw new InvalidExpressionException("Unable to create connection to the Database");
            return connection;
        }
    }
}