using System;

namespace _2026_Csh_Advanced.sprint10_Solid.D
{
    public static class DependencyInversionDemo
    {
        public static void Demo()
        {
            Console.WriteLine("--- D: Dependency Inversion Principle ---");
            IMessageSender smtp = new SmtpSender();
            var service = new NotificationService(smtp);
            service.Notify("Hello via SMTP");

            IMessageSender api = new ApiSender();
            service = new NotificationService(api);
            service.Notify("Hello via API");
            Console.WriteLine("(NotificationService залежить від IMessageSender, а не від конкретної реалізації)\n");
        }
    }

    public interface IMessageSender
    {
        void Send(string message);
    }

    public class SmtpSender : IMessageSender
    {
        public void Send(string message) => Console.WriteLine($"SMTP: {message}");
    }

    public class ApiSender : IMessageSender
    {
        public void Send(string message) => Console.WriteLine($"API: {message}");
    }

    public class NotificationService
    {
        private readonly IMessageSender _sender;
        public NotificationService(IMessageSender sender) => _sender = sender;
        public void Notify(string message) => _sender.Send(message);
    }
}