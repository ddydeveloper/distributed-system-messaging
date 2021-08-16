using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DeadLetter
{
    public class Program
    {
        private const string DlQueueName = "6_DL_Queue";
        private const string DlExchangeName = "DLX";

        public static void Main()
        {
            Receive();
        }

        private static void Receive()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var conn = factory.CreateConnection();
            using var channel = conn.CreateModel();

            channel.ExchangeDeclare(DlExchangeName, ExchangeType.Fanout, true);
            channel.QueueDeclare(DlQueueName, true, false, false);
            channel.QueueBind(DlQueueName, DlExchangeName, string.Empty);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var deathQueue = GetHeader(ea, "x-first-death-queue");
                var deathReason = GetHeader(ea, "x-first-death-reason");
                Console.WriteLine($"Dead letter: {message}\n   reason: {deathReason}\n   queue: {deathQueue}\n");
            };

            channel.BasicConsume(DlQueueName, true, consumer);

            Console.WriteLine("Press [enter] to stop reading dead letters.");
            Console.ReadLine();
        }

        private static string GetHeader(BasicDeliverEventArgs ea, string name)
        {
            var bytes = ea.BasicProperties.Headers[name] as byte[];
            return Encoding.UTF8.GetString(bytes!);
        }
    }
}