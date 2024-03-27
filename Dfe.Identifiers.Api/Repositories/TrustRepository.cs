using Dfe.Identifiers.Api.Context;
using Dfe.Identifiers.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Dfe.Identifiers.Api.Repositories
{
    public class TrustRepository : ITrustRepository
    {
        private MstrContext _context;

        public TrustRepository(MstrContext context)
        {
            _context = context;
        }

        public async Task<List<Trust>> GetTrustsByIdentifier(string identifier, CancellationToken cancellationToken)
        {
            var trusts = await DefaultIncludes().AsNoTracking().Where(x =>
                    identifier.Equals(x.UKPRN) || identifier.Equals(x.GroupID) || identifier.Equals(x.GroupUID))
                .ToListAsync(cancellationToken).ConfigureAwait(false);
            return trusts;
        }

        private IQueryable<Trust> DefaultIncludes()
        {
            var x = _context.Trusts
                .Include(x => x.TrustType)
                .AsQueryable();

            return x;
        }
    }
}