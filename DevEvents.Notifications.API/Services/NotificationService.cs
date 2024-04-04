namespace DevEvents.Notifications.API.Services
{
    public interface INotificationService
    {
        Task SendEmail(string email, string template, Dictionary<string, string> parameters);
        Task SendSms(string phoneNumber, string template, Dictionary<string, string> parameters);

    }

    public class NotificationService : INotificationService
    {
        public NotificationService()
        {
        }

        public Task SendEmail(string email, string template, Dictionary<string, string> parameters)
        {
            Console.WriteLine($"Email {template} enviado a {parameters["name"]}");

            return Task.CompletedTask;
        }

        public Task SendSms(string phoneNumber, string template, Dictionary<string, string> parameters)
        {
            Console.WriteLine($"SMS {template} enviado a {parameters["name"]}");

            return Task.CompletedTask;
        }
    }
}
