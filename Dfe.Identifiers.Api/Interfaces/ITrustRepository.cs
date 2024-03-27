using Dfe.Identifiers.Api.Models;

namespace Dfe.Identifiers.Api.Repositories;

public interface ITrustRepository
{
    Task<List<Trust>> GetTrustsByIdentifier(string identifier, CancellationToken cancellationToken);
}