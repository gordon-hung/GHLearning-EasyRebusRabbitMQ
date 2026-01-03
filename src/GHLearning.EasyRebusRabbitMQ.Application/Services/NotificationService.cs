using GHLearning.EasyRebusRabbitMQ.Application.Abstractions.Services;
using GHLearning.EasyRebusRabbitMQ.Application.Dtos;
using GHLearning.EasyRebusRabbitMQ.Doamin.Entities;
using GHLearning.EasyRebusRabbitMQ.Doamin.Events;
using GHLearning.EasyRebusRabbitMQ.Doamin.Repositories;
using GHLearning.EasyRebusRabbitMQ.SharedKernel;

namespace GHLearning.EasyRebusRabbitMQ.Application.Services;

internal sealed class NotificationService : INotificationService
{
	private readonly IGuidGenerator _guidGenerator;
	private readonly INotificationRepository _repository;
	private readonly IEventBus _bus;
	private readonly ISqlUnitOfWork _sqlUnitOfWork;

	public NotificationService(
		IGuidGenerator guidGenerator,
		INotificationRepository repository,
		IEventBus bus,
		ISqlUnitOfWork sqlUnitOfWork)
	{
		_guidGenerator = guidGenerator;
		_repository = repository;
		_bus = bus ?? throw new ArgumentNullException(nameof(bus));
		_sqlUnitOfWork = sqlUnitOfWork;
	}

	public async Task<Guid> SendNotificationAsync(NotificationDto dto)
	{
		var notification = new Notification(_guidGenerator.NextGuid(), dto.Title, dto.Message);

		// Repository 只加入 Entity 到 DbContext
		await _repository.AddAsync(notification).ConfigureAwait(ConfigureAwaitOptions.None);

		await _sqlUnitOfWork.SaveChangesAsync().ConfigureAwait(ConfigureAwaitOptions.None);


		// 發送 Rebus 事件
		var domainEvent = new NotificationSentEvent(notification.Id, notification.Title, notification.Message);
		await _bus.SendAsync("notification_bus", "notification_queue", domainEvent).ConfigureAwait(ConfigureAwaitOptions.None);

		return notification.Id;
	}

	public async Task<Notification?> GetNotificationAsync(Guid id)
	{
		return await _repository.GetByIdAsync(id).ConfigureAwait(ConfigureAwaitOptions.None);
	}

	public async Task<Guid> SendUrgentNotificationAsync(UrgentNotificationDto dto)
	{
		var notification = new Notification(_guidGenerator.NextGuid(), dto.Title, dto.Message);

		// Repository 只加入 Entity 到 DbContext
		await _repository.AddAsync(notification).ConfigureAwait(ConfigureAwaitOptions.None);

		await _sqlUnitOfWork.SaveChangesAsync().ConfigureAwait(ConfigureAwaitOptions.None);


		// 發送 Rebus 事件
		var domainEvent = new UrgentNotificationEvent(notification.Id, notification.Title, notification.Message);
		await _bus.SendAsync("urgent_notification_bus", "urgent_notification_queue", domainEvent).ConfigureAwait(ConfigureAwaitOptions.None);

		return notification.Id;
	}
}
