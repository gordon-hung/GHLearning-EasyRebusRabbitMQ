using GHLearning.EasyRebusRabbitMQ.Application.Dtos;
using GHLearning.EasyRebusRabbitMQ.Application.Services;
using GHLearning.EasyRebusRabbitMQ.Doamin.Entities;
using GHLearning.EasyRebusRabbitMQ.Doamin.Events;
using GHLearning.EasyRebusRabbitMQ.Doamin.Repositories;
using GHLearning.EasyRebusRabbitMQ.SharedKernel;
using NSubstitute;

namespace GHLearning.EasyRebusRabbitMQ.ApplicationTests;

public class NotificationServiceTests
{
	private readonly IGuidGenerator _guidGenerator = Substitute.For<IGuidGenerator>();
	private readonly INotificationRepository _repository = Substitute.For<INotificationRepository>();
	private readonly IEventBus _bus = Substitute.For<IEventBus>();
	private readonly ISqlUnitOfWork _sqlUnitOfWork = Substitute.For<ISqlUnitOfWork>();

	private readonly NotificationService _sut;

	public NotificationServiceTests()
	{
		_sut = new NotificationService(
			_guidGenerator,
			_repository,
			_bus,
			_sqlUnitOfWork);
	}

	[Fact]
	public async Task SendNotificationAsync_ShouldCreateNotification_SaveAndPublishEvent()
	{
		// Arrange
		var generatedId = Guid.NewGuid();
		_guidGenerator.NextGuid().Returns(generatedId);

		var dto = new NotificationDto
		{
			Title = "Test Title",
			Message = "Test Message"
		};

		Notification? savedNotification = null;
		_repository
			.AddAsync(Arg.Any<Notification>())
			.Returns(ci =>
			{
				savedNotification = ci.Arg<Notification>();
				return Task.CompletedTask;
			});

		// 假設 SaveChangesAsync 回傳 Task<int>
		_sqlUnitOfWork
			.SaveChangesAsync()
			.Returns(0);

		_bus
			.SendAsync(
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<NotificationSentEvent>())
			.Returns(Task.CompletedTask);

		// Act
		var resultId = await _sut.SendNotificationAsync(dto);

		// Assert
		Assert.Equal(generatedId, resultId);
		Assert.NotNull(savedNotification);
		Assert.Equal(generatedId, savedNotification!.Id);
		Assert.Equal(dto.Title, savedNotification.Title);
		Assert.Equal(dto.Message, savedNotification.Message);

		await _repository.Received(1).AddAsync(Arg.Any<Notification>());
		await _sqlUnitOfWork.Received(1).SaveChangesAsync();
		await _bus.Received(1).SendAsync(
			"notification_bus",
			"notification_queue",
			Arg.Is<NotificationSentEvent>(e =>
				e.NotificationId == generatedId &&
				e.Title == dto.Title &&
				e.Message == dto.Message));
	}

	[Fact]
	public async Task SendUrgentNotificationAsync_ShouldCreateNotification_SaveAndPublishUrgentEvent()
	{
		// Arrange
		var generatedId = Guid.NewGuid();
		_guidGenerator.NextGuid().Returns(generatedId);

		var dto = new UrgentNotificationDto
		{
			Title = "Urgent Title",
			Message = "Urgent Message"
		};

		Notification? savedNotification = null;
		_repository
			.AddAsync(Arg.Any<Notification>())
			.Returns(ci =>
			{
				savedNotification = ci.Arg<Notification>();
				return Task.CompletedTask;
			});

		// 同樣假設 SaveChangesAsync 回傳 Task<int>
		_sqlUnitOfWork
			.SaveChangesAsync()
			.Returns(0);

		_bus
			.SendAsync(
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<UrgentNotificationEvent>())
			.Returns(Task.CompletedTask);

		// Act
		var resultId = await _sut.SendUrgentNotificationAsync(dto);

		// Assert
		Assert.Equal(generatedId, resultId);
		Assert.NotNull(savedNotification);
		Assert.Equal(generatedId, savedNotification!.Id);
		Assert.Equal(dto.Title, savedNotification.Title);
		Assert.Equal(dto.Message, savedNotification.Message);

		await _repository.Received(1).AddAsync(Arg.Any<Notification>());
		await _sqlUnitOfWork.Received(1).SaveChangesAsync();
		await _bus.Received(1).SendAsync(
			"urgent_notification_bus",
			"urgent_notification_queue",
			Arg.Is<UrgentNotificationEvent>(e =>
				e.NotificationId == generatedId &&
				e.Title == dto.Title &&
				e.Message == dto.Message));
	}

	[Fact]
	public async Task GetNotificationAsync_ShouldReturnNotificationFromRepository()
	{
		// Arrange
		var id = Guid.NewGuid();
		var notification = new Notification(id, "title", "message");

		_repository
			.GetByIdAsync(id)
			.Returns(notification);

		// Act
		var result = await _sut.GetNotificationAsync(id);

		// Assert
		Assert.NotNull(result);
		Assert.Equal(id, result!.Id);

		await _repository.Received(1).GetByIdAsync(id);
	}
}