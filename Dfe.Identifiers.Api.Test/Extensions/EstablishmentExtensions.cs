using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Api.Test.Extensions;

public static class EstablishmentExtensions
{
    public static Establishment LinkLocalAuthorityToEstablishment(this Establishment establishment,
        LocalAuthority localAuthority)
    {
        establishment.LocalAuthority = localAuthority;
        return establishment;
    }
}