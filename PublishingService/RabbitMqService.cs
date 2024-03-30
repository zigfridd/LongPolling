using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DeliveryService
{
	public class RabbitMqService : IRabbitMqService
    {
        IModel _channel;

        public RabbitMqService(IModel channel)
        {
            _channel = channel;
        }

        public void SendMessage(object obj)
        {
            var message = JsonSerializer.Serialize(obj);
            SendMessage(message);
        }

        public void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            //публикация сообщения
            _channel.BasicPublish(exchange: "",
                           routingKey: _channel.CurrentQueue,
                           basicProperties: null,
                           body: body);
        }
    }
}

