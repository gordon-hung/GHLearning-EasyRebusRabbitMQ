using GHLearning.EasyRebusRabbitMQ.Doamin.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.EventHandlers;

public class UrgentNotificationEventHandler(
	ILogger<UrgentNotificationEventHandler> logger) : IHandleMessages<UrgentNotificationEvent>
{
	public Task Handle(UrgentNotificationEvent message)
	{
		logger.LogInformation($"[緊急通知] Id={message.NotificationId}, Title={message.Title}, Message={message.Message}");
		// 這裡可以做緊急通知處理，例如立即發 SMS 或 Slack
		return Task.CompletedTask;
	}
}