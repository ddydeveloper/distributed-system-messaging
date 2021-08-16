using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
{
    public static class Program
    {
        private const string QueueName = "1_Worker_Queue";

        public static void Main(string[] args)
        {
            Send(GetMessage(args));
        }

        private static void Send(string message)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.QueueDeclare(QueueName, true, false, false, null);
            // durable - will survive broker restart
            // exclusive - only accessed by the current connection and is deleted when connection closes
            // auto - will be deleted when all consumers have finished using it
            // arguments - TTL, queue limit, plugin settings

            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            // persistent - message will survive broker restart if not delivered

            channel.BasicPublish(string.Empty, QueueName, properties, body);
            Console.WriteLine("Message published.");
        }

        private static string GetMessage(IReadOnlyCollection<string> args)
        {
            return args.Count > 0 ? string.Join(" ", args) : "Unknown";
        }
    }
}