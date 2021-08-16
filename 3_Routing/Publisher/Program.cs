using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
{
    public static class Program
    {
        private const string ExchangeName = "3_Routing_Exchange";

        public static void Main(string[] args)
        {
            Send(args);
        }

        private static void Send(IReadOnlyCollection<string> args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);
            var body = Encoding.UTF8.GetBytes(GetMessage(args));

            channel.BasicPublish(ExchangeName, GetRoutingKey(args), null, body);
            Console.WriteLine("Message published.");
        }

        private static string GetRoutingKey(IReadOnlyCollection<string> args)
        {
            return args.Count > 0 ? args.First() : "unknown";
        }

        private static string GetMessage(IReadOnlyCollection<string> args)
        {
            return args.Count > 1 ? string.Join(" ", args.Skip(1)) : "Unknown";
        }
    }
}