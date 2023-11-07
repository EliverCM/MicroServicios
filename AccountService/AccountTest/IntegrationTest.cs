using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AccountTest
{
    public class IntegrationTest
    {

        [Fact]
        public void AccountService_EnviaRabbitQMTo_UserServiceQueue()
        {
            // Arrange
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Configura la conexión a RabbitMQ
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var queueName = "movimiento_queue";
            string MessageValidation = null;

            // Act: Envía un mensaje desde AccountService a la cola
            string MessageFromAccountService = "Mensaje de prueba desde Microservicio AccountService";
            var body = Encoding.UTF8.GetBytes(MessageFromAccountService);
            try
            {
                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                MessageValidation = MessageFromAccountService;
            }
            catch (Exception ex)
            {
                // Falla test
                MessageValidation = "";
            }

            Thread.Sleep(2000);

            // Assert: Verifica si los mensajes son iguales, que en caso de no publicacion falla Test
            Assert.Equal(MessageFromAccountService, MessageValidation);
        }

    }
}
