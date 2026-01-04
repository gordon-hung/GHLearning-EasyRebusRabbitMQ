using GHLearning.EasyRebusRabbitMQ.Application.DependencyInjection;
using GHLearning.EasyRebusRabbitMQ.Doamin.Events;
using GHLearning.EasyRebusRabbitMQ.Infrastructure;
using GHLearning.EasyRebusRabbitMQ.Infrastructure.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// EF Core
builder.Services.AddDbContext<NotificationDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// UnitOfWork
builder.Services.AddInfrastructure((sp, options) => options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

// Rebus
var rabbitUri = builder.Configuration.GetConnectionString("RabbitMQ");

builder.Services.AddRebus(configure => configure
	.Transport(t => t.UseRabbitMq(rabbitUri, "notification_service")),
	isDefaultBus: false,
	key: "notification_service_bus");

builder.Services.AddRebus(configure => configure
	.Transport(t => t.UseRabbitMq(rabbitUri, "notification_queue"))
	.Routing(r => r.TypeBased()
	.Map<NotificationSentEvent>("notification_queue")),
	isDefaultBus: false,
	key: "notification_bus");

builder.Services.AddRebus(configure => configure
	.Transport(t => t.UseRabbitMq(rabbitUri, "urgent_notification_queue"))
	.Routing(r => r.TypeBased()
	.Map<UrgentNotificationEvent>("urgent_notification_queue")),
	isDefaultBus: false,
	key: "urgent_notification_bus");


// Application Service
builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
	db.Database.EnsureCreated(); // 會自動建立資料表
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"));// swagger/
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();
/*
app.Lifetime.ApplicationStarted.Register(() =>
{
	var bus = app.Services.GetRequiredService<IBusRegistry>().GetBus("notification_service_bus");
	bus.Subscribe<NotificationSentEvent>().Wait();
});
*/
app.Run();
