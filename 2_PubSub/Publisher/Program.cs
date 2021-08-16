using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
{
    public static class Program
    {
        private const string ExchangeName = "2_PubSub_Exchange";

        public static void Main(string[] args)
        {
            Send(GetMessage(args));
        }

        private static void Send(string message)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(ExchangeName, string.Empty, null, body);
            Console.WriteLine("Message published.");
        }

        private static string GetMessage(IReadOnlyCollection<string> args)
        {
            return args.Count > 0 ? string.Join(" ", args) : "Hello World!";
        }
    }
}