using BancoApi.Domain;
using BancoApi.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BancoApi.Service.Receiver
{
    public class BancoCreateReceiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;
        private readonly ILogger<BancoCreateReceiver> _logger;
        public IServiceProvider Services { get; }

        public BancoCreateReceiver(
            IServiceProvider services,
            IOptions<RabbitMqConfiguration> rabbitMqOptions,
            ILogger<BancoCreateReceiver> logger)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = "BancoQueue"; //rabbitMqOptions.Value.QueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _logger = logger;
            Services = services;

            InitializeRabbitMqListener();
        }
        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password"
            };

            _connection = factory.CreateConnection();
            _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var banco = JsonConvert.DeserializeObject<Banco>(content);

                HandleMessage(banco);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerCancelled;

            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }
        private async void HandleMessage(Banco banco)
        {
            try
            {  //tiver problemas de scopo para o repositório.
               //resolvido com o link:
               //https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio#consuming-a-scoped-service-in-a-background-task
                using (var scope = Services.CreateScope())
                {
                    _logger.LogWarning($"Recuperando informações da fila e inserindo BD. Banco:{banco.Nome.ToUpper()}");
                   var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IBancoRepository>();
                    await scopedProcessingService.AddAsync(banco);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }

        private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
        {
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
        {
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
        }

    }
}
