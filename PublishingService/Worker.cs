using DeliveryService;

namespace PublishingService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    IRabbitMqService _rabbitService; //сервис для работы с брокером сообщений

    public Worker(ILogger<Worker> logger, IRabbitMqService rabbitMq)
    {
        _logger = logger;
        _rabbitService = rabbitMq;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            _rabbitService.SendMessage("current time: "+ DateTimeOffset.Now);

            await Task.Delay(1000, stoppingToken); //задержка в 5 сек.
        }
    }
}

