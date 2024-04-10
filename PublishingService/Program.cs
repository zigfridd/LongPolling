using DeliveryService;
using Npgsql;
using SharedLibraries;
using RabbitMQ.Client;

#region Configuration

IConfiguration configuration = new ConfigurationBuilder()
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.AddEnvironmentVariables()
.AddCommandLine(args)
.Build();
var hostName = configuration.GetSection("AppSettings")["host"];
var queueName = configuration.GetSection("AppSettings")["queue"];
var hostBuilder = Host.CreateDefaultBuilder(args);

#region PostgreSQL

var sqlHostName = configuration.GetSection("AppSettings")["sqlhost"];
var sqlUserName = configuration.GetSection("AppSettings")["sqluser"];
var sqlPassName = configuration.GetSection("AppSettings")["sqlpass"];
var sqlDbName = configuration.GetSection("AppSettings")["sqldb"];

#endregion
#endregion

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
        services.AddSingleton<IPostgreSQLService, PostgreSQLService>(services => new PostgreSQLService(sqlHostName, sqlUserName, sqlPassName, sqlDbName));

        services.AddHostedService<Worker>(serviceProvider
            => new Worker(serviceProvider.GetService<ILogger<Worker>>(),
            serviceProvider.GetService<IRabbitMqService>(), serviceProvider.GetService<IPostgreSQLService>()));
    })
    .Build();

    host.Run();
}

