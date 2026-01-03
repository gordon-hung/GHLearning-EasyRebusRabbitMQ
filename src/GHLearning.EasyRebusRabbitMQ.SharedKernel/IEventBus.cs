namespace GHLearning.EasyRebusRabbitMQ.SharedKernel
{
    public interface IEventBus
    {
		/// <summary>
		/// Publishes the asynchronous.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="event">The event.</param>
		/// <returns></returns>
		Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;

		/// <summary>
		/// Sends the asynchronous.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="destinationQueue">The destination queue.</param>
		/// <param name="event">The event.</param>
		/// <returns></returns>
		Task SendAsync<TEvent>(string destinationQueue, TEvent @event) where TEvent : class;

		/// <summary>
		/// Sends the asynchronous.
		/// </summary>
		/// <typeparam name="TEvent">The type of the event.</typeparam>
		/// <param name="key">The key.</param>
		/// <param name="destinationQueue">The destination queue.</param>
		/// <param name="event">The event.</param>
		/// <returns></returns>
		Task SendAsync<TEvent>(string key, string destinationQueue, TEvent @event) where TEvent : class;

	}
}
