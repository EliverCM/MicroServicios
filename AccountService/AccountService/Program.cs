using AccountDB;
using AccountService.Repositories;
using AccountService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AccountContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AccountConnection"))
);
builder.Services.AddScoped<RabbitMQService>();
builder.Services.AddScoped<IRepository<Movimiento>, MovimientoRepository>();
builder.Services.AddScoped<IRepository<Cuenta>, AccountRepository>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var Context = scope.ServiceProvider.GetRequiredService<AccountContext>();
    Context.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
