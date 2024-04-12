using Dfe.Identifiers.Api.Services;
using Dfe.Identifiers.Domain.Authentication;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dfe.Identifiers.Api.Test.Services;

public class ApiKeyServiceTests
{
    private static readonly ApiUser ValidApiUser = new() { ApiKey = "valid_key", UserName = "test validApiUser" };

    private readonly Dictionary<string, string?> _validConfiguration = new()
    {
        { AuthenticationConstants.APIKEYCONFIGURATIONPATH, JsonConvert.SerializeObject(ValidApiUser) }
    };

    private IConfigurationRoot BuildConfiguration(Dictionary<string, string?> configuration)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(configuration)
            .Build();
    }

    [Fact]
    public void Execute_Returns_Null_When_No_ApiKey_Exists()
    {
        var service = new ApiKeyService(BuildConfiguration(new Dictionary<string, string?>()));

        var result = service.Execute("valid_key");

        result.Should().BeNull();
    }

    [Fact]
    public void Execute_Returns_ApiUser_When_Matching_ApiKey_Found()
    {
        var service = new ApiKeyService(BuildConfiguration(_validConfiguration));

        var result = service.Execute(ValidApiUser.ApiKey);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(ValidApiUser);
    }

    [Fact]
    public void Execute_Returns_Null_When_No_Matching_ApiKey_Found()
    {
        var service = new ApiKeyService(BuildConfiguration(_validConfiguration));

        var result = service.Execute("invalid_key");

        result.Should().BeNull();
    }
}