
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace MRabbitMQ.subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://epjtctyq:eaL7weadEAYHY10IYRt3c3czj04Trwnf@moose.rmq.cloudamqp.com/epjtctyq");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            var queueName = "direct-queue-Critical";
            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Loglar dinleniyor");

            consumer.Received += (object? sender, BasicDeliverEventArgs e) =>
            {

                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine("Gelen Mesaj: " + message);

                File.AppendAllText("log-critical.txt", message + "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();



        }

    }
}