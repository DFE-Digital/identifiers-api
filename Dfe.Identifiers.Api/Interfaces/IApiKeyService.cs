using Dfe.Identifiers.Domain.Authentication;
using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Api.Interfaces;

public interface IApiKeyService
{
    ApiUser? Execute(string request);
}