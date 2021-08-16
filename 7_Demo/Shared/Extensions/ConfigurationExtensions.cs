using Microsoft.Extensions.Configuration;

namespace Messages.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetRabbitMq(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("RabbitMq");
        }

        public static string GetSeq(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("Seq");
        }

        public static string GetSql(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("SQL");
        }

        public static string GetBotToken(this IConfiguration configuration)
        {
            var token = configuration.GetConnectionString("BotToken");
            return token;
        }
        
        public static string GetChannel(this IConfiguration configuration)
        {
            return configuration.GetConnectionString("Channel");
        }
    }
}