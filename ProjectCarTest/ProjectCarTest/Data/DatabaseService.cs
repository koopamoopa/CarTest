using Microsoft.Data.Sqlite;
using System.Data;

namespace ProjectCarTest.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }
    }
}
