using MediatorService.Interfaces.IServices;
using RabbitMQ.Client;
using SharedDto;
using System.Text;

namespace MediatorService.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private const IBasicProperties RABBIT_MQ_BASIC_PROPERTIES = null;

        public bool SendEmail(string emailBody)
        {
            bool isSuccess = false;

            var factory = new ConnectionFactory() { HostName = SharedConstants.RABBIT_MQ_HOST };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: SharedConstants.RABBIT_MQ_EMAIL_QUEUE,
                                         durable: SharedConstants.RABBIT_MQ_DURABLE,
                                         exclusive: SharedConstants.RABBIT_MQ_EXCLUSIVE,
                                         autoDelete: SharedConstants.RABBIT_MQ_AUTODELETE,
                                         arguments: SharedConstants.RABBIT_MQ_ARGUMENTS);

                    byte[] body = Encoding.UTF8.GetBytes(emailBody);

                    channel.BasicPublish(exchange: SharedConstants.RABBIT_MQ_EXCHANGE,
                                         routingKey: SharedConstants.RABBIT_MQ_ROUTING_KEY,
                                         basicProperties: RABBIT_MQ_BASIC_PROPERTIES,
                                         body: body);

                    isSuccess = true;
                }
            }

            return isSuccess;
        }
    }
}
