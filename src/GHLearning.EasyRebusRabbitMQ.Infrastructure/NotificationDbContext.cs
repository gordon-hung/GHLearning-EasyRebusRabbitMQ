using GHLearning.EasyRebusRabbitMQ.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace GHLearning.EasyRebusRabbitMQ.Infrastructure;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationEntity>(entity =>
        {
            entity.ToTable("notifications"); // table name 必須正確
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
        });
    }
}