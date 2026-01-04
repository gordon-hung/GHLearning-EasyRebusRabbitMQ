using GHLearning.EasyRebusRabbitMQ.Doamin.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.EventHandlers;

public class NotificationSentHandler(
	ILogger<NotificationSentHandler> logger) : IHandleMessages<NotificationSentEvent>
{
	public async Task Handle(NotificationSentEvent message)
	{
		logger.LogInformation($"[收到通知事件] Id={message.NotificationId}");
	}
}