using AccountDB;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace AccountService.Services
{
    public class RabbitMQService
    {
        private readonly IConfiguration _configuration;

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void EnviarMensaje(MovimientoDTO movimiento)
        {
            var rabbitMQConfig = _configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

            var factory = new ConnectionFactory
            {
                HostName = rabbitMQConfig.HostName,
                Port = rabbitMQConfig.Port,
                UserName = rabbitMQConfig.UserName,
                Password = rabbitMQConfig.Password,
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "movimiento_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

                string jsonMessage = JsonConvert.SerializeObject(movimiento);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                channel.BasicPublish(exchange: "", routingKey: "movimiento_queue", basicProperties: null, body: body);
            }
        }
    }
}
