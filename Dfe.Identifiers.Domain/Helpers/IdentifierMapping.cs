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

    public static ConversionProjectIdentifiers MapConversionProjectToIdentifiers(ConversionProject project)
    {
        return new ConversionProjectIdentifiers(ApplicationReferenceNumber: project.ApplicationReferenceNumber,
            TrustReferenceNumber: project.TrustReferenceNumber, SponsorReferenceNumber: project.SponsorReferenceNumber);
    }
    
    public static TransferProjectIdentifiers MapTransferProjectToIdentifiers(TransferProject project)
    {
        return new TransferProjectIdentifiers(ProjectReference: project.ProjectReference);
    }
    
    public static FormAMatProjectIdentifiers MapFormAMatProjectToIdentifiers(FormAMatProject project)
    {
        return new FormAMatProjectIdentifiers(ReferenceNumber: project.ReferenceNumber, ApplicationReference: project.ApplicationReference);
    }
}