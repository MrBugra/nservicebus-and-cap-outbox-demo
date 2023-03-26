using CapOutboxSample.Processors;
using NServiceBusOutboxSample.Infrastructure;
using NServiceBusOutboxSample.Infrastructure.Middlewares;
using NServiceBusOutboxSample.Processors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add NServiceBus
builder.Host.AddNServiceBusHost();

//EF
builder.Services.AddEf();
    
// builder.Services.AddDbContext<WalletDbContext>(options =>
// {
//     options.UseNpgsql("Server=localhost;Port=5432;Database=Test;User Id=admin;Password=admin;");
// });

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IMessageProcessor, MessageProcessor>();

var app = builder.Build();
app.UseMiddleware<NServiceBusSessionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();