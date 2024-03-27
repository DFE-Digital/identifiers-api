using Dfe.Identifiers.Api.Context;
using Dfe.Identifiers.Api.Interfaces;
using Dfe.Identifiers.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Identifiers.Api.Repositories;

public class EstablishmentRepository : IEstablishmentRepository
{
    private MstrContext _context;

    public EstablishmentRepository(MstrContext context)
    {
        _context = context;
    }

    public async Task<List<Establishment>> GetEstablishmentsByIdentifier(string identifier,
        CancellationToken cancellationToken)
    {
        var establishments = await BaseQuery().Where(item =>
                identifier.Equals(item.Establishment.UKPRN) || identifier.Equals(item.Establishment.URN.ToString()) ||
                (item.Establishment.EstablishmentNumber != null &&
                 identifier.EndsWith(item.Establishment.EstablishmentNumber!.ToString()!) &&
                 item.LocalAuthority.Code != null && identifier.StartsWith(item.LocalAuthority.Code)))
            .ToListAsync(cancellationToken);
        var results = establishments.Select(ToEstablishment).ToList();
        return results;
    }

    private IQueryable<EstablishmentQueryResult> BaseQuery()
    {
        var result =
            from establishment in _context.Establishments
            from ifdPipeline in _context.IfdPipelines.Where(i => i.GeneralDetailsUrn == establishment.PK_GIAS_URN)
                .DefaultIfEmpty()
            from establishmentType in _context.EstablishmentTypes.Where(e => e.SK == establishment.EstablishmentTypeId)
                .DefaultIfEmpty()
            from localAuthority in _context.LocalAuthorities.Where(l => l.SK == establishment.LocalAuthorityId)
                .DefaultIfEmpty()
            select new EstablishmentQueryResult(
                Establishment: establishment, IfdPipeline: ifdPipeline, LocalAuthority: localAuthority,
                EstablishmentType: establishmentType
            );

        return result;
    }

    private static Establishment ToEstablishment(EstablishmentQueryResult queryResult)
    {
        var result = queryResult.Establishment;
        result.IfdPipeline = queryResult.IfdPipeline;
        result.LocalAuthority = queryResult.LocalAuthority;
        result.EstablishmentType = queryResult.EstablishmentType;

        return result;
    }
}

internal record EstablishmentQueryResult(
    Establishment Establishment,
    IfdPipeline IfdPipeline,
    LocalAuthority LocalAuthority,
    EstablishmentType EstablishmentType
);