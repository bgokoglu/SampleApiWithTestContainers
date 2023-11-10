using Microsoft.EntityFrameworkCore;
using SampleApiWithTestContainers.Infrastructure;
using SampleApiWithTestContainers.Product;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddDbContextFactory<ProductDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ProductDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")), optionsLifetime: ServiceLifetime.Singleton);

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

app.MapGet("/", async context => { await context.Response.WriteAsync("Hello, Minimal API!"); });

app.RegisterProductEndpoints();

//Database initialization
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetService<ProductDbContext>();
    app.Logger.LogInformation(context.Database.GetConnectionString());
    context.Database.Migrate();
}

app.Run();