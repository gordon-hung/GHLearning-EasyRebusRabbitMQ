using GHLearning.EasyRebusRabbitMQ.Doamin.Events;
using Rebus.Handlers;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.EventHandlers;

public class UrgentNotificationEventHandler : IHandleMessages<UrgentNotificationEvent>
{
    public Task Handle(UrgentNotificationEvent message)
    {
        Console.WriteLine($"[緊急通知] Id={message.NotificationId}, Title={message.Title}, Message={message.Message}");
        // 這裡可以做緊急通知處理，例如立即發 SMS 或 Slack
        return Task.CompletedTask;
    }
}