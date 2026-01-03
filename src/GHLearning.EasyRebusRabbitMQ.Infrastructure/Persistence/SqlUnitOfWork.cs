using GHLearning.EasyRebusRabbitMQ.SharedKernel;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure.Persistence;

internal sealed class SqlUnitOfWork : ISqlUnitOfWork
{
	private readonly NotificationDbContext _dbContext;

	public SqlUnitOfWork(NotificationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> _dbContext.SaveChangesAsync(cancellationToken);

}
