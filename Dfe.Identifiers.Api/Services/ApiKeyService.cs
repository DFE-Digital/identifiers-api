using Dfe.Identifiers.Api.Interfaces;
using Dfe.Identifiers.Domain.Authentication;
using Newtonsoft.Json;

namespace Dfe.Identifiers.Api.Services;

public class ApiKeyService(IConfiguration configuration) : IApiKeyService
{
    public ApiUser? Execute(string request)
    {

        var key = configuration
            .GetSection(AuthenticationConstants.APIKEYCONFIGURATIONPATH)
            .AsEnumerable()
            .Where(k => k.Value != null)
            .Select(k => JsonConvert.DeserializeObject<ApiUser>(k.Value))
            .FirstOrDefault(user => user.ApiKey.Equals(request));

        return key;
    }
}