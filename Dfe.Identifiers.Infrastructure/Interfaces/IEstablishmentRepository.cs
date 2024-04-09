using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Api.Interfaces;

public interface IEstablishmentRepository
{
    Task<List<Establishment>> GetEstablishmentsByIdentifier(string identifier,
        CancellationToken cancellationToken);
}