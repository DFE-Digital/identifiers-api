namespace Dfe.Identifiers.Domain.Identifiers;

public record IdentifiersCollection(
    TrustIdentifiers[] Trusts,
    EstablishmentIdentifiers[] Establishments,
    ConversionProjectIdentifiers[] ConversionProjects,
    TransferProjectIdentifiers[] TransferProjects,
    FormAMatProjectIdentifiers[] FormAMatProjects);