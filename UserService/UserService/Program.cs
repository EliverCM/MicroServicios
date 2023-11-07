using Microsoft.EntityFrameworkCore;
using UserDB;
using UserService.Controllers;
using UserService.Repositories;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddTransient<NotificationController>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection"))
);
builder.Services.AddScoped<RabbitMQService>();
builder.Services.AddScoped<IRepository<Notification>, NotificationRepository>();
builder.Services.AddScoped<IRepository<Cliente>, ClienteRepository>();

//Configuracion RabbitMQ
var serviceProvider = builder.Services.BuildServiceProvider();
var rabbitMQService = serviceProvider.GetRequiredService<RabbitMQService>();
rabbitMQService.InitializeRabbitMQ();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UserContext>();
    context.Database.Migrate();
}

app.UseSwagger();
app.MapControllers();
app.UseSwaggerUI();
app.Run();
