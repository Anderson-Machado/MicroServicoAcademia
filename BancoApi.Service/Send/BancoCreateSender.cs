using BancoApi.Domain;
using System;
using RabbitMQ.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace BancoApi.Service.Send
{
    public class BancoCreateSender : IBancoCreateSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;
        private readonly ILogger<BancoCreateSender> _logger;
        private readonly IConfiguration _config;

        public BancoCreateSender( ILogger<BancoCreateSender> logger, IConfiguration config)
        {
            _config = config;
         
            _queueName = "BancoQueue"; 
            _hostname = config.GetSection("RabbitMq:Hostname").Value == "localhost" ? "localhost": "172.18.0.3"; 
            _username = config.GetSection("RabbitMq:UserName").Value == "user"?"user":"user"; 
            _password = config.GetSection("RabbitMq:Hostname").Value == "Password"? "password": "password";
            _logger = logger;
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
                        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                        var json = JsonConvert.SerializeObject(banco);
                        var body = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
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
                _logger.LogInformation($"hostname:{_hostname}");
                _logger.LogInformation($"user:{_username}");
                _logger.LogInformation($"password:{_password}"); 
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
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
