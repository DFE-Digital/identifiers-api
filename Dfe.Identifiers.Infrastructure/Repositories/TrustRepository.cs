using Dfe.Identifiers.Api.Interfaces;
using Dfe.Identifiers.Domain.Models;
using Dfe.Identifiers.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Identifiers.Infrastructure.Repositories
{
    public class TrustRepository(MstrContext context) : ITrustRepository
    {
        public async Task<List<Trust>> GetTrustsByIdentifier(string identifier, CancellationToken cancellationToken)
        {
            var trusts = await DefaultIncludes().AsNoTracking().Where(x =>
                    identifier.Equals(x.UKPRN) || identifier.Equals(x.GroupID) || identifier.Equals(x.GroupUID))
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return trusts;
        }

        private IQueryable<Trust> DefaultIncludes()
        {
            var x = context.Trusts
                .Include(x => x.TrustType)
                .AsQueryable();

            return x;
        }
    }
}