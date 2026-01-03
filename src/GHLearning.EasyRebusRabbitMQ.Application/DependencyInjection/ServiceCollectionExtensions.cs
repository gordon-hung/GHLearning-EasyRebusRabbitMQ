using GHLearning.EasyRebusRabbitMQ.Application.Abstractions.Services;
using GHLearning.EasyRebusRabbitMQ.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GHLearning.EasyRebusRabbitMQ.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }
}
