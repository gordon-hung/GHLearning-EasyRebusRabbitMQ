namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.Entities;

public class NotificationEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string Message { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}