namespace GHLearning.EasyRebusRabbitMQ.SharedKernel;

public interface ISqlUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
