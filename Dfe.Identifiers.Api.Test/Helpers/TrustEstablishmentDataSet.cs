using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Api.Test.Helpers;

public record TrustEstablishmentDataSet(
    Trust Trust,
    List<Establishment> Establishments
);