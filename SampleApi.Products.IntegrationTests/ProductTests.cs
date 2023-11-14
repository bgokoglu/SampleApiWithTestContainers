using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using SampleApi.Products.Core;
using SampleApi.Products.IntegrationTests.Common;

namespace SampleApi.Products.IntegrationTests;

public class ProductTests : BaseIntegrationTest
{
    private readonly IntegrationTestWebAppFactory _factory;

    public ProductTests(IntegrationTestWebAppFactory factory)
        : base(factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("Test Product")]
    public async Task Create_ShouldCreateProduct(string productName)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        // insert into db what you want to assert
        var postResponse = await client.PostAsJsonAsync("api/products", new ProductRequest { Name = productName});
        
        // Assert
        // Assert that the HTTP response status code is 201 Created
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        // Deserialize the response content to get the product ID
        var postContent = await postResponse.Content.ReadAsStringAsync();
        var productId = JsonSerializer.Deserialize<string>(postContent);

        // Assert that the product ID is not null or empty
        productId.Should().NotBeNullOrEmpty();
        
        // Act
        // read from db
        var getResponse = await client.GetAsync($"api/products/{productId}");

        // Assert
        // Assert that the HTTP response status code is 200 OK
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Deserialize the response content to get the product ID
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var productResponse = JsonSerializer.Deserialize<ProductResponse>(getContent);
        
        // Assert that the product properties are equal to what was posted
        productResponse.Should().NotBeNull();
        productResponse!.Id.Should().Be(productId);
        productResponse.Name.Should().Be(productName);
    }
}