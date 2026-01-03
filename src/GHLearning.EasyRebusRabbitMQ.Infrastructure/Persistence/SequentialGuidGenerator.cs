using GHLearning.EasyRebusRabbitMQ.SharedKernel;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.Persistence;

internal sealed class SequentialGuidGenerator : IGuidGenerator
{
    public Guid NextGuid() => Guid.CreateVersion7(DateTimeOffset.UtcNow);
}
