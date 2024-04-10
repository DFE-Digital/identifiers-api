using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Infrastructure.Interfaces;

public interface ITrustRepository
{
    Task<List<Trust>> GetTrustsByIdentifier(string identifier, CancellationToken cancellationToken);
}