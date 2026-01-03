namespace GHLearning.EasyRebusRabbitMQ.Doamin.Entities;

public class Notification
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Constructor
    public Notification(Guid id, string title, string message)
    {
        Id = id;
        Title = title;
        Message = message;
        CreatedAt = DateTime.UtcNow;
    }

    // Update message example
    public void UpdateMessage(string message)
    {
        Message = message;
    }
}
