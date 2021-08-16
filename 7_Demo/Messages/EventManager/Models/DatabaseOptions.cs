namespace EventManager.Models
{
    public class DatabaseOptions
    {
        public DatabaseOptions(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}