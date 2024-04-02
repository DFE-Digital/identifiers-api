using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Domain.Query;

public record EstablishmentQueryResult
{
    public Establishment Establishment;
    public LocalAuthority LocalAuthority;
    public EstablishmentType? EstablishmentType;
};