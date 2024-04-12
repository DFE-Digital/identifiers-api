using Dfe.Identifiers.Domain.Identifiers;
using Dfe.Identifiers.Infrastructure.Interfaces;
using static Dfe.Identifiers.Domain.Helpers.IdentifierMapping;

namespace Dfe.Identifiers.Application;

public class IdentifiersQuery(ITrustRepository trustRepository, IEstablishmentRepository establishmentRepository) : IIdentifiersQuery
{
    private ITrustRepository _trustRepository { get; } = trustRepository;
    private IEstablishmentRepository _establishmentRepository { get; } = establishmentRepository;

    public async Task<IdentifiersCollection> GetIdentifiers(string identifier, CancellationToken cancellationToken)
    {
        var trusts = await _trustRepository.GetTrustsByIdentifier(identifier, cancellationToken).ConfigureAwait(false);
        var establishments = await _establishmentRepository.GetEstablishmentsByIdentifier(identifier, cancellationToken)
            .ConfigureAwait(false);
        var results = new IdentifiersCollection(trusts.Select(MapTrustToIdentifiers).ToArray(),
            establishments.Select(MapEstablishmentToIdentifiers).ToArray());
        return results;
    }
}