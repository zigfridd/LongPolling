using DeliveryService;
using PublishingService;
using RabbitMQ.Client;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();
var hostName = configuration.GetSection("AppSettings")["host"];
var queueName = configuration.GetSection("AppSettings")["queue"];
var hostBuilder = Host.CreateDefaultBuilder(args);

var factory = new ConnectionFactory() { HostName = hostName };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: queueName,
                   durable: false,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null);

    var host = hostBuilder.ConfigureServices(services =>
    {
        services.AddSingleton<IRabbitMqService, RabbitMqService>(services => new RabbitMqService(channel));

        services.AddHostedService<Worker>(serviceProvider
            => new Worker(serviceProvider.GetService<ILogger<Worker>>(),
            serviceProvider.GetService<IRabbitMqService>()));
    })
    .Build();

    host.Run();
}

