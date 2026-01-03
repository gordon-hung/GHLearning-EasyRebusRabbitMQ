using GHLearning.EasyRebusRabbitMQ.Doamin.Entities;

namespace GHLearning.EasyRebusRabbitMQ.Doamin.Repositories;

public interface INotificationRepository
{
    Task AddAsync(Notification notification);
    Task<Notification?> GetByIdAsync(Guid id);
}