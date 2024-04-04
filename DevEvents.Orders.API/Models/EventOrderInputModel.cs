namespace DevEvents.Orders.API.Models
{
    public class EventOrderInputModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public decimal Price { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
    }

    public class EventOrderCompleted
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public decimal Price { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
