using System.Reflection;
using Common.Api.ErrorHandling;
using Common.Core.Repository;
using Common.Core.SystemClock;
using Common.Infrastructure;
using Common.Infrastructure.Mediator;
using SampleApiWithTestContainers.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSystemClock();
builder.Services.AddCommonInfrastructure();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddMediator(Assembly.GetExecutingAssembly());

//product services
builder.Services.AddProductValidations();
builder.Services.AddProductRepository();
builder.Services.AddProductDatabase(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandling();

app.MapGet("/", async context => { await context.Response.WriteAsync("Hello, Minimal API!"); });
app.RegisterProductEndpoints();

app.UseProductDatabase();

app.Run();

public partial class Program { }