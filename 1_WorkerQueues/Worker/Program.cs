using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Worker
{
    public static class Program
    {
        private const string QueueName = "1_Worker_Queue";

        public static void Main(string[] args)
        {
            Receive(args[0].Replace("_", " "));
        }

        private static void Receive(string name)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.QueueDeclare(QueueName, true, false, false, null);
            // durable - will survive broker restart
            // exclusive - only accessed by the current connection and is deleted when connection closes
            // autoDelete - will be deleted when all consumers have finished using it
            // arguments - TTL, queue limit, plugin settings

            channel.BasicQos(0, 1, false);
            // prefetchCount - not to give more than one message to a worker at a time

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var firstDotIdx = message.IndexOf(".", StringComparison.Ordinal);

                var text = firstDotIdx != -1
                    ? message.Substring(0, firstDotIdx)
                    : message;

                var dots = message.Split('.').Length - 1;

                Console.WriteLine($"New speaker applied: {text}");
                Thread.Sleep(dots * 1000);
                Console.WriteLine($"[x] Handled {(dots > 0 ? dots : 1)} hours later.\n");
                
                ((EventingBasicConsumer) sender)?.Model.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume(QueueName, false, consumer);

            Console.WriteLine($"{name} is listening...\n");
            Console.ReadLine();
        }
    }
}