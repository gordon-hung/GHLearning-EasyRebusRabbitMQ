using GHLearning.EasyRebusRabbitMQ.Doamin.Entities;

namespace GHLearning.EasyRebusRabbitMQ.DoaminTests;

public class NotificationTests
{
	[Fact]
	public void Constructor_SetsPropertiesAndCreatedAtUtc()
	{
		// Arrange
		var id = Guid.NewGuid();
		var title = "測試標題";
		var message = "初始訊息";
		var before = DateTime.UtcNow;

		// Act
		var notification = new Notification(id, title, message);
		var after = DateTime.UtcNow;

		// Assert
		Assert.Equal(id, notification.Id);
		Assert.Equal(title, notification.Title);
		Assert.Equal(message, notification.Message);
		Assert.InRange(notification.CreatedAt, before.AddSeconds(-1), after.AddSeconds(1));
		Assert.Equal(DateTimeKind.Utc, notification.CreatedAt.Kind);
	}

	[Fact]
	public void UpdateMessage_ChangesMessage_PreservesOtherProperties()
	{
		// Arrange
		var id = Guid.NewGuid();
		var title = "標題";
		var message = "訊息";
		var notification = new Notification(id, title, message);

		// Act
		var newMessage = "已更新的訊息";
		notification.UpdateMessage(newMessage);

		// Assert
		Assert.Equal(newMessage, notification.Message);
		Assert.Equal(id, notification.Id);
		Assert.Equal(title, notification.Title);
	}
}
