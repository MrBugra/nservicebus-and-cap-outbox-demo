using CapOutboxSample.Data;
using CapOutboxSample.Infrastructure;
using CapOutboxSample.Processors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IMessageProcessor,MessageProcessor>();

//EF
builder.Services.AddDbContext<WalletDbContext>(options =>
    options.UseNpgsql("Server=localhost;Port=5432;Database=Test;User Id=admin;Password=admin;"));

builder.Services.AddCapServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();