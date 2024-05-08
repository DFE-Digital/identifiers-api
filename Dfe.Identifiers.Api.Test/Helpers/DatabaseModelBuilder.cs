using AutoFixture;
using Dfe.Identifiers.Api.Test.Constants;
using Dfe.Identifiers.Api.Test.Extensions;
using Dfe.Identifiers.Domain.Models;
using Dfe.Identifiers.Infrastructure.Context;

namespace Dfe.Identifiers.Api.Test.Helpers;

public static class DatabaseModelBuilder
{
    private static readonly Fixture _fixture = new();

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

    public static ConversionProject BuildConversionProject()
    {
        var result = _fixture.Create<ConversionProject>();
        result.Id = null;
        result.FormAMatProjectId = null;
        result.FormAMatProject = null;
        return result;
    }
    
    public static TransferProject BuildTransferProject()
    {
        var result = _fixture.Create<TransferProject>();
        result.Id = null;
        return result;
    }
    
    public static FormAMatProject BuildFormAMatProject()
    {
        var result = _fixture.Create<FormAMatProject>();
        result.Id = null;
        return result;
    }
    
    public static async Task<List<Trust>> BuildTrustSet(MstrContext context)
    {
        var trusts = new List<Trust>();

        for (var idx = 0; idx < 3; idx++)
        {
            var trust = BuildTrust();
            trusts.Add(trust);
        }

        context.Trusts.AddRange(trusts);
        await context.SaveChangesAsync();
        return trusts;
    }

    public static async Task<List<Trust>> CreateDuplicateTrustSet(MstrContext context, TrustIdTypes trustId)
    {
        var trusts = new List<Trust>();

        for (var idx = 0; idx < 3; idx++)
        {
            var trust = BuildTrust();
            switch (trustId)
            {
                case TrustIdTypes.GroupID:
                    trust.GroupID = "GroupID";
                    break;
                case TrustIdTypes.UKPRN:
                    trust.UKPRN = "UKPRN";
                    break;
                case TrustIdTypes.GroupUID:
                    trust.GroupUID = "GroupUID";
                    break;
            }

            trusts.Add(trust);
        }

        context.Trusts.AddRange(trusts);
        await context.SaveChangesAsync();
        return trusts;
    }

    public static async Task<List<Trust>> CreateTrustSetWithEmptyData(MstrContext context,
        TrustIdTypes trustIdTypeToKeep)
    {
        var trusts = await BuildTrustSet(context);
        foreach (var trust in trusts)
        {
            switch (trustIdTypeToKeep)
            {
                case TrustIdTypes.GroupID:
                    trust.UKPRN = null;
                    break;
                case TrustIdTypes.UKPRN:
                    trust.GroupID = null;
                    break;
                case TrustIdTypes.GroupUID:
                    trust.GroupID = null;
                    trust.UKPRN = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trustIdTypeToKeep), trustIdTypeToKeep, null);
            }
        }

        context.Trusts.UpdateRange(trusts);
        await context.SaveChangesAsync();
        return trusts;
    }
    
    public static TrustEstablishmentDataSet CreateEstablishmentSet(MstrContext context)
    {
        var trust = BuildTrust();
        context.Add(trust);
        context.SaveChanges();

        var establishments = new List<Establishment>();

        for (var idx = 0; idx < 3; idx++)
        {
            var localAuthority = context.LocalAuthorities.First(la => la.SK % 3 == idx);
            var establishment = BuildEstablishment().LinkLocalAuthorityToEstablishment(localAuthority);

            context.Establishments.Add(establishment);

            establishments.Add(establishment);
        }

        context.SaveChanges();

        var trustToEstablishmentLinks =
            LinkTrustToEstablishments(trust, establishments);

        context.EducationEstablishmentTrusts.AddRange(trustToEstablishmentLinks);

        context.SaveChanges();

        var result = new TrustEstablishmentDataSet(Trust: trust, Establishments: establishments);

        return result;
    }

    public static TrustEstablishmentDataSet CreateSameUKPRNDataSet(MstrContext context)
    {
        var trust = BuildTrust();
        trust.UKPRN = IdentifierConstants.MixedSameUkprn;
        context.Add(trust);
        context.SaveChanges();

        //Establishment
        var establishments = new List<Establishment>();

        var establishment = BuildEstablishment().LinkLocalAuthorityToEstablishment(context.LocalAuthorities.First());
        establishment.UKPRN = IdentifierConstants.MixedSameUkprn;

        context.Establishments.Add(establishment);

        establishments.Add(establishment);

        context.SaveChanges();

        var trustToEstablishmentLinks =
            LinkTrustToEstablishments(trust, establishments);

        context.EducationEstablishmentTrusts.AddRange(trustToEstablishmentLinks);

        context.SaveChanges();

        var result = new TrustEstablishmentDataSet(Trust: trust, Establishments: establishments);

        return result;
    }

    private static List<EducationEstablishmentTrust> LinkTrustToEstablishments(Trust trust,
        List<Establishment> establishments)
    {
        var result = new List<EducationEstablishmentTrust>();

        establishments.ForEach(establishment =>
        {
            var educationEstablishmentTrust = new EducationEstablishmentTrust()
            {
                TrustId = (int)trust.SK,
                EducationEstablishmentId = (int)establishment.SK
            };

            result.Add(educationEstablishmentTrust);
        });

        return result;
    }

    public static async  Task<List<ConversionProject>> CreateConversionProjectsSet(AcademisationContext context)
    {
        var projects = new List<ConversionProject>();

        for (var idx = 0; idx < 3; idx++)
        {
            var project = BuildConversionProject();
            projects.Add(project);
        }

        context.ConversionProjects.AddRange(projects);
        await context.SaveChangesAsync();
        return projects;
    }
    
    public static async  Task<List<TransferProject>> CreateTransferProjectsSet(AcademisationContext context)
    {
        var projects = new List<TransferProject>();

        for (var idx = 0; idx < 3; idx++)
        {
            var project = BuildTransferProject();
            projects.Add(project);
        }

        context.TransferProjects.AddRange(projects);
        await context.SaveChangesAsync();
        return projects;
    }
    
    public static async  Task<List<FormAMatProject>> CreateFormAMatProjectsSet(AcademisationContext context)
    {
        var projects = new List<FormAMatProject>();

        for (var idx = 0; idx < 3; idx++)
        {
            var project = BuildFormAMatProject();
            projects.Add(project);
        }

        context.FormAMatProjects.AddRange(projects);
        await context.SaveChangesAsync();
        return projects;
    }
}