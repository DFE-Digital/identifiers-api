using Dfe.Identifiers.Domain.Models;
using Dfe.Identifiers.Infrastructure.Context;
using Dfe.Identifiers.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Identifiers.Infrastructure.Repositories
{
    public class TrustRepository(MstrContext context) : ITrustRepository
    {
        public async Task<List<Trust>> GetTrustsByIdentifier(string identifier, CancellationToken cancellationToken)
        {
            var trusts = await DefaultIncludes().AsNoTracking().Where(trust =>
                    identifier.Equals(trust.UKPRN) || identifier.Equals(trust.GroupID) || identifier.Equals(trust.GroupUID))
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return trusts;
        }

        private IQueryable<Trust> DefaultIncludes()
        {
            var query = context.Trusts
                .Include(trust => trust.TrustType)
                .AsQueryable();

            return query;
        }
    }
}