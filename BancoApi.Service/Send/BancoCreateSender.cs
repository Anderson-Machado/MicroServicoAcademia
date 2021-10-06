using BancoApi.Domain;
using BancoApi.Service.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace BancoApi.Service.Send
{
    public class BancoCreateSender : IBancoCreateSender
    {

        private IConnection _connection;
        private readonly ILogger<BancoCreateSender> _logger;
        private readonly IConfiguration _config;
        private readonly RabbitMqConfiguration _options;


        public BancoCreateSender(ILogger<BancoCreateSender> logger, IConfiguration config, IOptionsMonitor<RabbitMqConfiguration> options)
        {
            _config = config;
            _logger = logger;
            _options = options.CurrentValue;
            CreateConnection();
        }
        public void SendBanco(Banco banco)
        {
            if (ConnectionExists())
            {
                try
                {

                    using (var channel = _connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: _options.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                        var json = JsonConvert.SerializeObject(banco);
                        var body = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(exchange: "", routingKey: _options.QueueName, basicProperties: null, body: body);
                        _logger.LogInformation("Enviado para fila");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao Tentar enviar para fila:{ex.Message}");

                }
            }
            else
            {
                _logger.LogInformation($"Conexão não está ativa.");

            }
        }
        private void CreateConnection()
        {
            try
            {
                _logger.LogInformation("Tentando criar conexão da fila com seguinte informações");
                _logger.LogInformation($"hostname:{_options.Hostname}");
                _logger.LogInformation($"user:{_options.Hostname}");
                _logger.LogInformation($"password:{_options.Password}");
                var factory = new ConnectionFactory
                {
                    HostName = _options.Hostname,
                    UserName = _options.UserName,
                    Password = _options.Password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro criar conexão da fila:{ex.Message}");

            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
