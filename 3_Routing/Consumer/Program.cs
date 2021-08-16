using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consumer
{
    public static class Program
    {
        private const string ExchangeName = "3_Routing_Exchange";

        public static void Main(string[] args)
        {
            Receive(args);
        }

        private static void Receive(string[] args)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct);

            var queueName = channel.QueueDeclare().QueueName;
            var (name, routes) = ExtractArgs(args);

            foreach (var key in routes) channel.QueueBind(queueName, ExchangeName, key);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"{ea.RoutingKey}: {message}.\n");

                //((EventingBasicConsumer) sender)?.Model.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine($"{name} is listening...\n");
            Console.ReadLine();
        }

        private static (string, string[]) ExtractArgs(IReadOnlyList<string> args)
        {
            var name = args[0].Replace("_", " ");
            var routingKeys = args.Skip(1).ToArray();
            return (name, routingKeys);
        }
    }
}