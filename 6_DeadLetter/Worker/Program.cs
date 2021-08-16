using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Worker
{
    public static class Program
    {
        private const string QueueName = "6_Worker_Queue";

        public static void Main()
        {
            Receive();
        }

        private static void Receive()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            var arguments = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", "DLX"}
            };

            channel.QueueDeclare(QueueName, true, false, false, arguments);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"Message received: {message}.\n");
                ((EventingBasicConsumer) sender)?.Model.BasicReject(ea.DeliveryTag, false);
            };

            channel.BasicConsume(QueueName, false, consumer);

            Console.WriteLine("Press [enter] to stop reading messages.");
            Console.ReadLine();
        }
    }
}