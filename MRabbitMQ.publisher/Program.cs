
using RabbitMQ.Client;
using System.Text;

namespace MRabbitMQ.publisher
{

    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }
    class Program
    {

        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://epjtctyq:eaL7weadEAYHY10IYRt3c3czj04Trwnf@moose.rmq.cloudamqp.com/epjtctyq");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            //   channel.QueueDeclare("hello-queue", true, false, false);

            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
            
            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("format", "pdf");
            headers.Add("shape", "a4");

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;
            properties.Persistent = true;

            channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes("header mesajım"));


            Console.ReadLine();
        }
    }
}