using RabbitMQ.Client;

namespace MRabbitMQWeb.ExcelCreate.Services
{
    public class RabbitMQClientService : IDisposable
    {
        private readonly ILogger<RabbitMQClientService> _logger;

        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "ExcelDirectExchange";
        public static string RoutingExcel = "excel-route-file";
        public static string QueueName = "queue-excel-file";

        public RabbitMQClientService(ILogger<RabbitMQClientService> logger, ConnectionFactory connectionFactory)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
        }

        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();

            if (_channel is { IsOpen: true })
            {
                return _channel;
            }

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, type: "direct", true, false);

            _channel.QueueDeclare(QueueName, true, false, false, null);

            _channel.QueueBind(exchange: ExchangeName, queue: QueueName, routingKey: RoutingExcel);

            _logger.LogInformation("RabbitMQ ile bağlantı kuruldu...");

            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();

            _connection?.Close();
            _connection?.Dispose();

            _logger.LogInformation("RabbitMQ ile bağlantı koptu...");
        }
    }

}
