using GHLearning.EasyRebusRabbitMQ.Doamin.Repositories;
using GHLearning.EasyRebusRabbitMQ.Infrastructure.EventHandlers;
using GHLearning.EasyRebusRabbitMQ.Infrastructure.Persistence;
using GHLearning.EasyRebusRabbitMQ.Infrastructure.Repositories;
using GHLearning.EasyRebusRabbitMQ.SharedKernel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rebus.Config;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbContextOptions)
    {
        services.AddDbContext<NotificationDbContext>(dbContextOptions);
        services.AddScoped<IGuidGenerator, SequentialGuidGenerator>();
        services.AddScoped<ISqlUnitOfWork, SqlUnitOfWork>();
        services.AddScoped<IEventBus, RebusEventBus>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

		services.AutoRegisterHandlersFromAssemblyOf<NotificationSentEventHandler>();
		services.AutoRegisterHandlersFromAssemblyOf<UrgentNotificationEventHandler>();

		return services;
    }
}
