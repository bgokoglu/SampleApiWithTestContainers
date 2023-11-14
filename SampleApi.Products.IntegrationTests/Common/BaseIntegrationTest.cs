using Microsoft.Extensions.DependencyInjection;
using SampleApi.Products.Infrastructure.Database;

namespace SampleApi.Products.IntegrationTests.Common;

public abstract class BaseIntegrationTest
    : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly ProductPersistence _dbContext;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _dbContext = _scope.ServiceProvider.GetRequiredService<ProductPersistence>();
    }

    public void Dispose()
    {
        _scope.Dispose();
        _dbContext.Dispose();
        
        GC.SuppressFinalize(this);
    }
}