using System.Net;
using Dfe.Identifiers.Api.Interfaces;
using Dfe.Identifiers.Api.Middleware;
using Dfe.Identifiers.Api.Services;
using Dfe.Identifiers.Domain.Authentication;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Dfe.Identifiers.Api.Test.Middleware;

public class ApiKeyMiddlewareTests
{
    private readonly ApiUser validUser = new() { ApiKey = "valid_key", UserName = "test user" };
    public async Task<HttpClient> SetupTestServer()
    {
        var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddScoped<IApiKeyService, ApiKeyService>();
                    })
                    .Configure(app =>
                    {
                        app.UseMiddleware<ApiKeyMiddleware>();
                    }).ConfigureAppConfiguration(configuration =>
                    {
                        var inMemorySettings = new Dictionary<string, string?>
                        {
                            { AuthenticationConstants.APIKEYCONFIGURATIONPATH, JsonConvert.SerializeObject(validUser) }
                        };
                        configuration.AddInMemoryCollection(inMemorySettings);
                    });
            })
            .StartAsync();

        var server = host.GetTestClient();
        server.BaseAddress = new Uri("https://test.server/");
        return server;
    } 
    
    [Fact]
    public async Task No_ApiKey_Returns_NotAuthorized()
    {
        using var server = await SetupTestServer();
        
        var request = new HttpRequestMessage(HttpMethod.Get, "/api");

        var context = await server.SendAsync(request);

        context.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var content = await context.Content.ReadAsStringAsync();
        content.Should().Be("Api Key was not provided.");
    }
    
    [Fact]
    public async Task Random_ApiKey_Returns_NotAuthorized()
    {
        using var server = await SetupTestServer();
        
        var request = new HttpRequestMessage(HttpMethod.Get, "/api");
        request.Headers.Add(AuthenticationConstants.APIKEYNAME, "random key");

        var context = await server.SendAsync(request);
        
        context.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var content = await context.Content.ReadAsStringAsync();
        content.Should().Be("Unauthorized client.");
    }
    
    [Fact]
    public async Task Correct_ApiKey_Returns_NotFound()
    {
        using var server = await SetupTestServer();
        
        var request = new HttpRequestMessage(HttpMethod.Get, "/api");
        request.Headers.Add(AuthenticationConstants.APIKEYNAME, validUser.ApiKey);
        
        var context = await server.SendAsync(request);

        context.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task No_ApiKey_without_ApiRoute_Returns_Unauthorized()
    {
        using var server = await SetupTestServer();
        
        var request = new HttpRequestMessage(HttpMethod.Get, "/");
        
        var context = await server.SendAsync(request);

        context.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task No_ApiKey_with_healthRoute_Returns_NotFound()
    {
        using var server = await SetupTestServer();
        
        var request = new HttpRequestMessage(HttpMethod.Get, "/health");
        
        var context = await server.SendAsync(request);

        // Look for not found as we don't have the health endpoint setup
        // 404 proves that we don't need auth to reach the endpoint
        context.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}