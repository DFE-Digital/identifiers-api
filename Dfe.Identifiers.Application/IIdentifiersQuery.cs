using Dfe.Identifiers.Domain.Identifiers;

namespace Dfe.Identifiers.Application;

public interface IIdentifiersQuery
{
    Task<IdentifiersCollection> GetIdentifiers(string identifier, CancellationToken cancellationToken);
}