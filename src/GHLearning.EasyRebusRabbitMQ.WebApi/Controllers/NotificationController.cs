using GHLearning.EasyRebusRabbitMQ.Application.Abstractions.Services;
using GHLearning.EasyRebusRabbitMQ.Application.Dtos;
using GHLearning.EasyRebusRabbitMQ.Doamin.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GHLearning.EasyRebusRabbitMQ.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
	private readonly INotificationService _notificationService;

	// 注入 INotificationService
	public NotificationController(INotificationService notificationService)
	{
		_notificationService = notificationService;
	}

	/// <summary>
	/// 發送通知
	/// POST /api/notification
	/// </summary>
	[HttpPost("notification")]
	public async Task<IActionResult> SendNotificationAsync([FromBody] NotificationDto dto)
	{
		// 呼叫 Service
		Guid notificationId = await _notificationService.SendNotificationAsync(dto).ConfigureAwait(ConfigureAwaitOptions.None);

		return Ok(new
		{
			NotificationId = notificationId,
			Message = "Notification sent successfully."
		});
	}

	/// <summary>
	/// 發送通知
	/// POST /api/notification
	/// </summary>
	[HttpPost("urgent-notification")]
	public async Task<IActionResult> SendUrgentNotificationAsync([FromBody] UrgentNotificationDto dto)
	{
		// 呼叫 Service
		Guid notificationId = await _notificationService.SendUrgentNotificationAsync(dto).ConfigureAwait(ConfigureAwaitOptions.None);

		return Ok(new
		{
			NotificationId = notificationId,
			Message = "Urgent Notification sent successfully."
		});
	}

	/// <summary>
	/// 取得通知
	/// GET /api/notification/{id}
	/// </summary>
	[HttpGet("{id}")]
	public async Task<IActionResult> GetNotification(Guid id)
	{
		// 呼叫 Service
		Notification? notification = await _notificationService.GetNotificationAsync(id).ConfigureAwait(false);

		if (notification == null)
			return NotFound(new { Message = "Notification not found." });

		return Ok(new
		{
			notification.Id,
			notification.Title,
			notification.Message,
			notification.CreatedAt
		});
	}
}