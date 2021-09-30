namespace BancoApi.Service.Configurations
{
    public class RabbitMqConfiguration
    {
        public const string BaseConfig = "RabbitMqConfiguration";
        public string Hostname { get; set; }
        public string QueueName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Enable { get; set; }

        //RabbitMqConfiguration() { }

    }
}
