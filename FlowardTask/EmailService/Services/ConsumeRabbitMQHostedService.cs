using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SharedDto;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailService.Services
{
    public class ConsumeRabbitMQHostedService : BackgroundService
    {
        private readonly ILogger _logger;
        private IConnection _connection;
        private IModel _channel;
        private Random Rnd { get; }
        private IConfiguration Config { get; }

        public ConsumeRabbitMQHostedService(ILoggerFactory loggerFactory, IConfiguration config)
        {
            this._logger = loggerFactory.CreateLogger<ConsumeRabbitMQHostedService>();
            Config = config;
            Rnd = new Random();

            InitRabbitMQ();
        }

        private void InitRabbitMQ()
        {
            var factory = new ConnectionFactory { HostName = SharedConstants.RABBIT_MQ_HOST };

            // create connection  
            _connection = factory.CreateConnection();

            // create channel  
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(SharedConstants.RABBIT_MQ_EXCHANGE, ExchangeType.Topic);
            _channel.QueueDeclare(
                SharedConstants.RABBIT_MQ_EMAIL_QUEUE,
                SharedConstants.RABBIT_MQ_DURABLE,
                SharedConstants.RABBIT_MQ_EXCLUSIVE,
                SharedConstants.RABBIT_MQ_AUTODELETE,
                SharedConstants.RABBIT_MQ_ARGUMENTS);
            _channel.QueueBind(
                SharedConstants.RABBIT_MQ_EMAIL_QUEUE,
                SharedConstants.RABBIT_MQ_EXCHANGE,
                SharedConstants.RABBIT_MQ_ROUTING_KEY,
                SharedConstants.RABBIT_MQ_ARGUMENTS);
            _channel.BasicQos(SharedConstants.RABBIT_MQ_PREFETCHSIZE, SharedConstants.RABBIT_MQ_PREFETCHCOUNT, SharedConstants.RABBIT_MQ_GLOBAL);

            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                // received message
                byte[] bodyBytes = ea.Body.ToArray();
                string body = Encoding.UTF8.GetString(bodyBytes);

                // handle the received message  
                HandleMessage(body, body);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(SharedConstants.RABBIT_MQ_EMAIL_QUEUE, SharedConstants.RABBIT_MQ_AUTOACKL, consumer);
            return Task.CompletedTask;
        }

        private void HandleMessage(string subject, string body)
        {
            SendEmail(subject, body);
            _logger.LogInformation($"consumer received {subject} : {body}");
        }

        private void SendEmail(string subject, string body)
        {
            string senderEmail = RandomEmail();
            string recieverEmail = RandomEmail();
            string senderHost = Config.GetValue<string>("EmailSettings:SenderHost");
            int senderPort = Config.GetValue<int>("EmailSettings:SenderPort");
            string senderLogin = Config.GetValue<string>("EmailSettings:Login");
            string senderPassword = Config.GetValue<string>("EmailSettings:Password");

            MailAddress fromEmail = new MailAddress(senderEmail);
            MailAddress toEmail = new MailAddress(recieverEmail);
            MailMessage message = new MailMessage(fromEmail, toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;

            SmtpClient smtp = new SmtpClient(senderHost, senderPort);
            smtp.Credentials = new NetworkCredential(senderLogin, senderPassword);
            smtp.EnableSsl = true;
            smtp.Send(message);
        }

        private string RandomEmail()
        {
            string email = "";
            while(email.Length < 10)
            {
                email += (char)Rnd.Next((int)'a', (int)'z');
            }

            email += "@";

            while (email.Length < 16)
            {
                email += (char)Rnd.Next((int)'a', (int)'z');
            }

            email += ".com";

            return email;
        }

        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
