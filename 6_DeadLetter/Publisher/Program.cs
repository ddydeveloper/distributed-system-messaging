using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace Publisher
{
    public static class Program
    {
        private const string QueueName = "6_Worker_Queue";

        public static void Main(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            var arguments = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", "DLX"}
            };

            channel.QueueDeclare(QueueName, true, false, false, arguments);

            for (var i = 1; i <= 10; i++) Send($"Message # {i}", channel);
        }

        private static void Send(string message, IModel channel)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(string.Empty, QueueName, properties, body);
            Console.WriteLine("Message published.");
        }
    }
}