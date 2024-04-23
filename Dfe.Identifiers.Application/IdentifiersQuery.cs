using Dfe.Identifiers.Domain.Identifiers;
using Dfe.Identifiers.Infrastructure.Interfaces;
using static Dfe.Identifiers.Domain.Helpers.IdentifierMapping;

namespace Dfe.Identifiers.Application;

public class IdentifiersQuery(
    ITrustRepository trustRepository,
    IEstablishmentRepository establishmentRepository,
    IProjectsRepository projectsRepository) : IIdentifiersQuery
{
    public async Task<IdentifiersCollection> GetIdentifiers(string identifier, CancellationToken cancellationToken)
    {
        var trusts = await trustRepository.GetTrustsByIdentifier(identifier, cancellationToken).ConfigureAwait(false);
        var establishments = await establishmentRepository.GetEstablishmentsByIdentifier(identifier, cancellationToken)
            .ConfigureAwait(false);
        var transfers = await projectsRepository.GetTransfersProjectsByIdentifier(identifier, cancellationToken)
            .ConfigureAwait(false);
        var conversions = await projectsRepository.GetConversionProjectsByIdentifier(identifier, cancellationToken)
            .ConfigureAwait(false);
        var formAMats = await projectsRepository.GetFormAMatProjectsByIdentifier(identifier, cancellationToken)
            .ConfigureAwait(false);

        var results = new IdentifiersCollection(trusts.Select(MapTrustToIdentifiers).ToArray(),
            establishments.Select(MapEstablishmentToIdentifiers).ToArray(),
            conversions.Select(MapConversionProjectToIdentifiers).ToArray(),
            transfers.Select(MapTransferProjectToIdentifiers).ToArray(),
            formAMats.Select(MapFormAMatProjectToIdentifiers).ToArray());
        return results;
    }
}