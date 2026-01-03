using GHLearning.EasyRebusRabbitMQ.Doamin.Events;
using Rebus.Handlers;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.EventHandlers;

public class NotificationSentEventHandler : IHandleMessages<NotificationSentEvent>
{
    public Task Handle(NotificationSentEvent message)
    {
        Console.WriteLine($"[普通通知] Id={message.NotificationId}");
        // 這裡可以做寫入 DB、發送 Email、推送通知等
        return Task.CompletedTask;
    }
}
