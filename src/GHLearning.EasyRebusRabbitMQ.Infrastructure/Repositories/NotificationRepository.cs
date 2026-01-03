using GHLearning.EasyRebusRabbitMQ.Doamin.Entities;
using GHLearning.EasyRebusRabbitMQ.Doamin.Repositories;
using GHLearning.EasyRebusRabbitMQ.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.Repositories;

internal sealed class NotificationRepository(NotificationDbContext context) : INotificationRepository
{
	public Task AddAsync(Notification notification)
    {
        var entity = MapToEntity(notification);
        return context.Notifications.AddAsync(entity).AsTask();
    }

    public async Task<Notification?> GetByIdAsync(Guid id)
    {
        var entity = await context.Notifications.FirstOrDefaultAsync(n => n.Id == id).ConfigureAwait(false);
        return entity == null ? null : MapToDomain(entity);
    }

    // EF Core DbContext 暴露給 Service 層用於 SaveChangesAsync
    public NotificationDbContext GetDbContext() => context;

    private static NotificationEntity MapToEntity(Notification domain)
    {
        return new NotificationEntity
        {
            Id = domain.Id,
            Title = domain.Title,
            Message = domain.Message,
            CreatedAt = domain.CreatedAt
        };
    }

    private static Notification MapToDomain(NotificationEntity entity)
    {
        var notification = new Notification(entity.Id,entity.Title, entity.Message);
        typeof(Notification).GetProperty("Id")!.SetValue(notification, entity.Id);
        typeof(Notification).GetProperty("CreatedAt")!.SetValue(notification, entity.CreatedAt);
        return notification;
    }
}