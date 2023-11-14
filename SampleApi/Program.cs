using System.Reflection;
using SampleApi.Common.Api.Authentication;
using SampleApi.Common.Api.ErrorHandling;
using SampleApi.Common.Core.SystemClock;
using SampleApi.Common.Infrastructure;
using SampleApi.Common.Infrastructure.Mediator;
using Microsoft.OpenApi.Models;
using SampleApi.Products.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApiAuthorization();
builder.Services.AddSystemClock();
builder.Services.AddCommonInfrastructure();

//product services
builder.Services.AddProducts(builder.Configuration, "Products");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Sample API", Version = "v1" });

    // Define the security scheme
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API key needed to access the endpoints. Include it in the 'X-Api-Key' header.",
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    // Define the security requirement
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

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

app.RegisterProducts("Products");

app.Run();

public partial class Program { }