using DevEvents.Orders.API.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace DevEvents.Orders.API.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class EventsController : ControllerBase
    {
        private readonly IModel _channel;
        private const string EXCHANGE = "events";
        private const string ROUTING_KEY = "order-created";

        public EventsController()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection("producer-dotlive-rabbitmq");

            _channel = connection.CreateModel();
        }

        [HttpPost("{id}/orders")]
        public IActionResult PostOrder(int id, EventOrderInputModel model)
        {
            var @event = new EventOrderCompleted
            {
                EventId = model.EventId,
                EventName = model.EventName,
                Price = model.Price,
                FullName = model.FullName,
                Email = model.Email
            };

            var json = JsonSerializer.Serialize(@event);

            var byteArray = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(EXCHANGE, ROUTING_KEY, null, byteArray);

            return Ok();
        }
    }
}
