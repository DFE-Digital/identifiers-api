using Dfe.Identifiers.Api.Models;

namespace Dfe.Identifiers.Api.Interfaces;

public interface IEstablishmentRepository
{
    Task<List<Establishment>> GetEstablishmentsByIdentifier(string identifier,
        CancellationToken cancellationToken);
}