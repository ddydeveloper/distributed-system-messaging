using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using EventManager.Models;

namespace EventManager.Services
{
    public interface IDbService
    {
        Task InsertContent(string content);

        Task<int> GetMessagesCount();

        Task RemoveMessages();
    }

    public class DbService : IDbService
    {
        private readonly DatabaseOptions _databaseOptions;

        public DbService(DatabaseOptions databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        public async Task InsertContent(string content)
        {
            await using var conn = new SqlConnection(_databaseOptions.ConnectionString);
            var sql = $"INSERT INTO messages.dbo.messages (content) VALUES (N'{content}');";
            await conn.ExecuteAsync(sql);
        }

        public async Task<int> GetMessagesCount()
        {
            await using var conn = new SqlConnection(_databaseOptions.ConnectionString);
            return await conn.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM messages.dbo.messages;");
        }

        public async Task RemoveMessages()
        {
            await using var conn = new SqlConnection(_databaseOptions.ConnectionString);
            await conn.ExecuteAsync("DELETE FROM messages.dbo.messages WHERE ID > -1;");
        }
    }
}