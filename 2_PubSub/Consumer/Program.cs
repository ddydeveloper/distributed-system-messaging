using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public static class Program
    {
        private const string ExchangeName = "2_PubSub_Exchange";

        public static void Main(string[] args)
        {
            Receive(args[0].Replace("_", " "));
        }

        private static void Receive(string name)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout);

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, ExchangeName, string.Empty);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Update: {message}\n");
            };

            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine($"{name} is listening...\n");
            Console.ReadLine();
        }
    }
}