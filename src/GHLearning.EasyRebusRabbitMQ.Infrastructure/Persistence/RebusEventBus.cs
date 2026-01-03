using GHLearning.EasyRebusRabbitMQ.SharedKernel;
using Rebus.ServiceProvider;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.Persistence;

internal sealed class RebusEventBus : IEventBus
{
	private readonly IBusRegistry _busRegistry;

	public RebusEventBus(IBusRegistry busRegistry)
	{
		_busRegistry = busRegistry;
	}

	public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
		=> _busRegistry.GetBus(null).Publish(@event);

	public Task SendAsync<TEvent>(string destinationQueue, TEvent @event) where TEvent : class
		=> _busRegistry.GetBus(null).Advanced.Routing.Send(destinationQueue, @event);

	public Task SendAsync<TEvent>(string key, string destinationQueue, TEvent @event) where TEvent : class
		=> _busRegistry.GetBus(key).Advanced.Routing.Send(destinationQueue, @event);
}