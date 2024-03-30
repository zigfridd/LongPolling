using DeliveryService;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var hubConnection = new HubConnectionBuilder()
        .WithUrl("http://localhost:5078/messageHub")
        .Build();
hubConnection.Closed += async (error) =>
{
    await Task.Delay(new Random().Next(0, 5) * 1000);
    await hubConnection.StartAsync();
};

var hostName = configuration.GetSection("AppSettings")["host"];
var queueName = configuration.GetSection("AppSettings")["queue"];

var factory = new ConnectionFactory() { HostName = hostName };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: queueName,
                   durable: false,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null);
    IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RabbitMqListener>(services => new RabbitMqListener(connection, channel, hubConnection));
    })
    .Build();

    host.Run();
}

