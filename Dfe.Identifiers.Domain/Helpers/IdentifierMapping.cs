using Dfe.Identifiers.Domain.Identifiers;
using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Domain.Helpers;

public static class IdentifierMapping
{
    public static EstablishmentIdentifiers MapEstablishmentToIdentifiers(Establishment establishment)
    {
        return new EstablishmentIdentifiers(UKPRN: establishment.UKPRN, URN: establishment.URN.ToString(),
            LAESTAB: $"{establishment.LocalAuthority.Code}/{establishment.EstablishmentNumber}");
    }

    public static TrustIdentifiers MapTrustToIdentifiers(Trust trust)
    {
        return new TrustIdentifiers(UID: trust.GroupUID, UKPRN: trust.UKPRN, TrustReference: trust.GroupID);
    }
}