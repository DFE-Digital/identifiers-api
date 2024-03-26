using Dfe.Identifiers.Api.Models;

namespace Dfe.Identifiers.Api.Repositories;

public interface IEstablishmentRepository
{
    Task<List<Establishment>> GetEstablishmentsByIdentifier(string identifier,
        CancellationToken cancellationToken);
}