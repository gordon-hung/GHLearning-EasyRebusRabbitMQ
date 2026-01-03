using GHLearning.EasyRebusRabbitMQ.Application.Dtos;
using GHLearning.EasyRebusRabbitMQ.Doamin.Entities;

namespace GHLearning.EasyRebusRabbitMQ.Application.Abstractions.Services;

public interface INotificationService
{
    Task<Guid> SendNotificationAsync(NotificationDto dto);
	Task<Guid> SendUrgentNotificationAsync(UrgentNotificationDto dto);
	Task<Notification?> GetNotificationAsync(Guid id);
}
