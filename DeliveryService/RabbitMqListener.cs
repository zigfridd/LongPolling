using System.Text;
using Microsoft.AspNetCore.SignalR.Client;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DeliveryService
{
    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection; //подключение к брокеру сообщений
        private IModel _channel;
        private HubConnection _hubConnection;

        public RabbitMqListener(IConnection connection, IModel channel, HubConnection hubConnection)
        {
            _channel = channel;
            _connection = connection;
            _hubConnection = hubConnection;
        }

        /// <summary>
        /// Асинхронный метод для отслеживания новых сообщений.
        /// </summary>
        /// <param name="stoppingToken">Токен отмены операции.</param>
        /// <returns>Экземпляр асинхронной задачи.</returns>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                SendToHub(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(_channel.CurrentQueue, false, consumer);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Метод отправляет сообщение на веб-приложение.
        /// </summary>
        /// <param name="message">Текст сообщения.</param>
        private async void SendToHub(string message)
        {
            while(_hubConnection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _hubConnection.StartAsync();
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(DateTime.Now + " - возникла ошибка при подключении к веб-приложению.");
                    await Task.Delay(2000); //задержка в 2 сек.
                    continue;
                }
            }
            if(_hubConnection.State == HubConnectionState.Connected)
                await _hubConnection.InvokeAsync("SendMessage", message);
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}

