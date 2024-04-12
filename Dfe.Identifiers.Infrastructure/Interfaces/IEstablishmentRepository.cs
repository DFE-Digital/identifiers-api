using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Infrastructure.Interfaces;

public interface IEstablishmentRepository
{
    Task<List<Establishment>> GetEstablishmentsByIdentifier(string identifier,
        CancellationToken cancellationToken);
}