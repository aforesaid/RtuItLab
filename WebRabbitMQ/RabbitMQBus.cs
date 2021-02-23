using System;
using System.Text;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace WebRabbitMQ
{
    public class RabbitMQBus : IEventBus
    {
        private readonly IHostApplicationLifetime _lifetime;
        public RabbitMQBus(IHostApplicationLifetime lifetime)
        {
            _lifetime = lifetime;
        }
        public void Publish(string message)
        {
            var factory = new ConnectionFactory
            {
                HostName = "",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
            using var connection = factory.CreateConnection();
            using var channel    = connection.CreateModel();
            channel.QueueDeclare("eshopq", false, false, false);
            var byteMess = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("","eshopq",body:byteMess);
            Console.WriteLine($"Message Published {message}");
        }

        public void Subscribe()
        {
            _lifetime.ApplicationStarted.Register(SubscribeProcess);
        }
        public void SubscribeProcess()
        {
            var factory = new ConnectionFactory
            {
                HostName = "",
                UserName = "guest",
                Password = "guest",
                Port     = 5672
            };
            var connection = factory.CreateConnection();
            var channel    = connection.CreateModel();
            channel.QueueDeclare("eshopq", false, false, false);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ((s, e) =>
            {
                var body    = e.Body.ToArray();
                var mess = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message subscribed {mess}");
            });
            channel.BasicConsume("eshopq", autoAck :true, consumer :consumer);
        }
    }
}
