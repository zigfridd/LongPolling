using DeliveryService;

namespace SharedLibraries;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    IRabbitMqService _rabbitService; //сервис для работы с брокером сообщений
    IPostgreSQLService _sqLService;

    public Worker(ILogger<Worker> logger, IRabbitMqService rabbitMq, IPostgreSQLService sqLService)
    {
        _logger = logger;
        _rabbitService = rabbitMq;
        _sqLService = sqLService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var text = "current time: " + DateTimeOffset.Now;
            _rabbitService.SendMessage(text);
            _sqLService.AddMessage(text, DateTime.Now);

            await Task.Delay(3000, stoppingToken); //задержка в 5 сек.
        }
    }
}

