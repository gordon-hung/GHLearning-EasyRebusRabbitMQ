namespace GHLearning.EasyRebusRabbitMQ.Doamin.Events;

public class UrgentNotificationEvent
{
    public Guid NotificationId { get; }
    public string Title { get; }
    public string Message { get; }

    public UrgentNotificationEvent(Guid notificationId, string title, string message)
    {
        NotificationId = notificationId;
        Title = title;
        Message = message;
    }
}