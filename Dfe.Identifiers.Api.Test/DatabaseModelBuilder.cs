using AutoFixture;
using Dfe.Identifiers.Domain.Models;

namespace Dfe.Identifiers.Api.Test
{
    public static class DatabaseModelBuilder
    {
        private static readonly Fixture _fixture = new Fixture();

        public static Trust BuildTrust()
        {
            var result = _fixture.Create<Trust>();
            result.SK = null;
            result.TrustStatus = "Open";
            result.TrustTypeId = 30;
            result.TrustType = null;
            result.TrustStatusId = null;
            result.RegionId = null;
            result.TrustBandingId = null;
            result.RID = result.RID.Substring(0, 10);

            return result;
        }

        public static Establishment BuildEstablishment()
        {
            var result = _fixture.Create<Establishment>();
            result.SK = null;
            result.LocalAuthority = null;
            result.EstablishmentType = null;
            result.PK_GIAS_URN = _fixture.Create<int>().ToString();
            // Only 224 or 228 are valid in this subset of test data used (see mstr context)
            result.EstablishmentTypeId = 224; 
            result.LocalAuthorityId = 1;

            return result;
        }
    }
}