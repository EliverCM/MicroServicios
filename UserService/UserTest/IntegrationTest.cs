using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading.Tasks;

namespace UserTest
{
    public class IntegrationTests
    {
        [Fact]
        public void UserService_RecibeRabbitMQFrom_AccountService()
        {
            // Arrange
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Configura la conexión a RabbitMQ
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var queueName = "movimiento_queue";

            // Act: Mensaje sincronizado con AccountService para TEST
            string MessageValidation = "Mensaje de prueba desde Microservicio AccountService";

            // Act: Espera y recibe un mensaje de la cola de Microservicio A
            var consumer = new EventingBasicConsumer(channel);
            string MessageFromAccountService = null;
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                MessageFromAccountService = Encoding.UTF8.GetString(body.ToArray());
            };
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            
            Thread.Sleep(2000);
            // Compara el mensaje recibido con el Sincronizado entre microservicios para el test
            Assert.Equal(MessageValidation, MessageFromAccountService);
        }

    }

}
