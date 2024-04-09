using Dfe.Identifiers.Domain.Models;
using Dfe.Identifiers.Domain.Query;

namespace Dfe.Identifiers.Domain.Helpers;

public static class QueryMapping
{
    public static Establishment ConvertQueryResultToEstablishment(EstablishmentQueryResult queryResult)
    {
        var result = queryResult.Establishment;
        result.LocalAuthority = queryResult.LocalAuthority;
        result.EstablishmentType = queryResult.EstablishmentType;

        return result;
    }
}