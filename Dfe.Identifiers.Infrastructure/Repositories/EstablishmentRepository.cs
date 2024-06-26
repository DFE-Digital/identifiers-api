﻿using Dfe.Identifiers.Domain.Models;
using Dfe.Identifiers.Domain.Query;
using Dfe.Identifiers.Infrastructure.Context;
using Dfe.Identifiers.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Dfe.Identifiers.Domain.Helpers.QueryMapping;

namespace Dfe.Identifiers.Infrastructure.Repositories;

public class EstablishmentRepository(MstrContext context) : IEstablishmentRepository
{
    public async Task<List<Establishment>> GetEstablishmentsByIdentifier(string identifier,
        CancellationToken cancellationToken)
    {
        var establishments = await BaseQuery().Where(establishment =>
                establishment.Establishment.UKPRN == identifier || establishment.Establishment.URN.ToString() == identifier ||
                identifier.StartsWith(establishment.LocalAuthority.Code) && identifier.EndsWith(establishment.Establishment.EstablishmentNumber.ToString())
            )
            .ToListAsync(cancellationToken);
        var results = establishments.Select(ConvertQueryResultToEstablishment).ToList();
        return results;
    }
    private IQueryable<EstablishmentQueryResult> BaseQuery()
    {
        var query = from establishment in context.Establishments
            from establishmentType in context.EstablishmentTypes.Where(establishmentType => establishmentType.SK == establishment.EstablishmentTypeId).DefaultIfEmpty()
            from localAuthority in context.LocalAuthorities.Where(localAuthority => localAuthority.SK == establishment.LocalAuthorityId).DefaultIfEmpty()
            select new EstablishmentQueryResult { Establishment = establishment, LocalAuthority = localAuthority, EstablishmentType = establishmentType };

        return query;
    }
}