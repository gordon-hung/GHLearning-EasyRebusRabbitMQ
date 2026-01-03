namespace GHLearning.EasyRebusRabbitMQ.Doamin.Events;

public class NotificationSentEvent
{
    public Guid NotificationId { get; }
    public string Title { get; }
    public string Message { get; }

    public NotificationSentEvent(Guid notificationId, string title, string message)
    {
        NotificationId = notificationId;
        Title = title;
        Message = message;
    }
}
