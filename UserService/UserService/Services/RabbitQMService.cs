using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using UserDB;
using UserService.Controllers;
using System.Text;


namespace UserService.Services
{

    public class RabbitMQService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly RabbitMQConfig _rabbitMQConfig;

        public RabbitMQService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _rabbitMQConfig = configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();
        }

        public void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMQConfig.HostName,
                Port = _rabbitMQConfig.Port,
                UserName = _rabbitMQConfig.UserName,
                Password = _rabbitMQConfig.Password,
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            // Configura el consumidor
            channel.QueueDeclare(queue: "movimiento_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var notificationController = _serviceProvider.GetRequiredService<NotificationController>();

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                // Ejecuta el método GetNotificaciones del controlador NotificationController
                await notificationController.PostNotificacion(message);
            };

            channel.BasicConsume(queue: "movimiento_queue", autoAck: true, consumer: consumer);
        }
    }
}
