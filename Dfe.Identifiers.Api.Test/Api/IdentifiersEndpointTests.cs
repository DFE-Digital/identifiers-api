using System.Net;
using Dfe.Identifiers.Api.Controllers;
using Dfe.Identifiers.Api.Test.Extensions;
using Dfe.Identifiers.Application;
using Dfe.Identifiers.Domain.Identifiers;
using Dfe.Identifiers.Domain.Models;
using Dfe.Identifiers.Infrastructure.Context;
using Dfe.Identifiers.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Dfe.Identifiers.Api.Test.Api;

public class IdentifiersEndpointTests : IClassFixture<ApiTestFixture>
{
    private ApiTestFixture Fixture { get; }
    private IdentifiersController Sut { get; }

    private const string MixedSameUkprn = "SameUKPRN";

    public IdentifiersEndpointTests(ApiTestFixture fixture)
    {
        Fixture = fixture;
        var logger = new Mock<ILogger<IdentifiersController>>();
        var context = fixture.GetMstrContext();
        Sut = new IdentifiersController(logger.Object, new IdentifiersQuery(new TrustRepository(context),
            new EstablishmentRepository(context)));
    }

    // TRUSTS

    [Theory]
    [InlineData(TrustIdTypes.GroupID)]
    [InlineData(TrustIdTypes.UKPRN)]
    [InlineData(TrustIdTypes.GroupUID)]
    public async Task Get_TrustIdentifiers_AndTrustExists_Returns_Ok(TrustIdTypes trustIdType)
    {
        using var context = Fixture.GetMstrContext();

        var trustData = await BuildTrustSet(context);

        var selectedTrust = trustData.First();

        var identifier = trustIdType switch
        {
            TrustIdTypes.GroupID => selectedTrust.GroupID,
            TrustIdTypes.UKPRN => selectedTrust.UKPRN,
            TrustIdTypes.GroupUID => selectedTrust.GroupUID,
            _ => throw new ArgumentOutOfRangeException(nameof(trustIdType), trustIdType, null)
        };

        var cancellationToken = new CancellationToken();
        var response = await Sut.GetIdentifiers(identifier!, cancellationToken);

        response.Result.Should().NotBeNull();
        response.Result.Should().BeOfType(typeof(OkObjectResult));
        response.GetStatusCode().Should().Be((int)HttpStatusCode.OK);

        var content = response.GetIdentifiers();
        content.Should().NotBeNull();
        var trusts = content!.Trusts;
        trusts.Length.Should().Be(1);
        AssertTrustIdentifierResponse(trusts.First(), selectedTrust);
    }

    [Theory]
    [InlineData(TrustIdTypes.GroupID)]
    [InlineData(TrustIdTypes.UKPRN)]
    [InlineData(TrustIdTypes.GroupUID)]
    public async Task Get_TrustIdentifiers_AndDuplicateTrustsExists_Returns_Ok(TrustIdTypes trustIdType)
    {
        using var context = Fixture.GetMstrContext();

        var trustData = await BuildDuplicateTrustSet(context, trustIdType);

        var identifier = trustIdType switch
        {
            TrustIdTypes.GroupID => "GroupID",
            TrustIdTypes.UKPRN => "UKPRN",
            TrustIdTypes.GroupUID => "GroupUID",
            _ => throw new ArgumentOutOfRangeException(nameof(trustIdType), trustIdType, null)
        };

        var cancellationToken = new CancellationToken();
        var response = await Sut.GetIdentifiers(identifier, cancellationToken);

        response.Result.Should().NotBeNull();
        response.Result.Should().BeOfType(typeof(OkObjectResult));
        response.GetStatusCode().Should().Be((int)HttpStatusCode.OK);

        var content = response.GetIdentifiers();
        content.Should().NotBeNull();
        var trusts = content!.Trusts;
        trusts.Length.Should().Be(3);
        AssertTrustIdentifierResponse(trusts.First(), trustData.First());
        AssertTrustIdentifierResponse(trusts[1], trustData[1]);
        AssertTrustIdentifierResponse(trusts[2], trustData[2]);
    }

    [Theory]
    [InlineData(TrustIdTypes.GroupID)]
    [InlineData(TrustIdTypes.UKPRN)]
    [InlineData(TrustIdTypes.GroupUID)]
    public async Task Get_TrustIdentifiers_AndNoOtherIdentifiersExist_Returns_Ok(TrustIdTypes trustIdType)
    {
        using var context = Fixture.GetMstrContext();

        var trustData = await BuildTrustSetWithEmptyData(context, trustIdType);
        var selectedTrust = trustData.First();
        var identifier = trustIdType switch
        {
            TrustIdTypes.GroupID => selectedTrust.GroupID,
            TrustIdTypes.UKPRN => selectedTrust.UKPRN,
            TrustIdTypes.GroupUID => selectedTrust.GroupUID,
            _ => throw new ArgumentOutOfRangeException(nameof(trustIdType), trustIdType, null)
        };

        var cancellationToken = new CancellationToken();
        var response = await Sut.GetIdentifiers(identifier!, cancellationToken);

        response.Result.Should().NotBeNull();
        response.Result.Should().BeOfType(typeof(OkObjectResult));
        response.GetStatusCode().Should().Be((int)HttpStatusCode.OK);

        var content = response.GetIdentifiers();
        content.Should().NotBeNull();
        var trusts = content!.Trusts;
        trusts.Length.Should().Be(1);
        AssertTrustIdentifierResponse(trusts.First(), selectedTrust);
    }

    [Fact]
    public async Task Get_TrustIdentifiers_AndTrustDoesNotExist_Returns_EmptyList()
    {
        using var context = Fixture.GetMstrContext();

        await BuildTrustSet(context);

        var cancellationToken = new CancellationToken();
        var response = await Sut.GetIdentifiers("NoTrustExists", cancellationToken);

        response.Result.Should().NotBeNull();
        response.Result.Should().BeOfType(typeof(OkObjectResult));
        response.GetStatusCode().Should().Be((int)HttpStatusCode.OK);

        var content = response.GetIdentifiers();
        content.Should().NotBeNull();
        var trusts = content!.Trusts;
        trusts.Length.Should().Be(0);
    }

    // ESTABLISHMENTS

    [Theory]
    [InlineData(EstablishmentsIdTypes.URN)]
    [InlineData(EstablishmentsIdTypes.UKPRN)]
    [InlineData(EstablishmentsIdTypes.LAESTAB)]
    public async Task Get_EstablishmentIdentifiers_AndEstablishmentExists_Returns_Ok(EstablishmentsIdTypes idType)
    {
        using var context = Fixture.GetMstrContext();

        var trustData = CreateEstablishmentSet(context);

        var selectedEstablishment = trustData.Establishments.First();
        var identifier = idType switch
        {
            EstablishmentsIdTypes.URN => $"{selectedEstablishment.URN}",
            EstablishmentsIdTypes.UKPRN => selectedEstablishment.UKPRN,
            EstablishmentsIdTypes.LAESTAB =>
                $"{selectedEstablishment.LocalAuthority.Code}%2F{selectedEstablishment.EstablishmentNumber}",
            _ => throw new ArgumentOutOfRangeException(nameof(idType), idType, null)
        };

        var cancellationToken = new CancellationToken();
        var response = await Sut.GetIdentifiers(identifier!, cancellationToken);

        response.Result.Should().NotBeNull();
        response.Result.Should().BeOfType(typeof(OkObjectResult));
        response.GetStatusCode().Should().Be((int)HttpStatusCode.OK);

        var content = response.GetIdentifiers();
        content.Should().NotBeNull();
        var establishments = content!.Establishments;
        establishments.Length.Should().Be(1);
        AssertEstablishmentsIdentifierResponse(establishments.First(), selectedEstablishment);
    }

    // Mixed 
    [Fact]
    public async Task Get_Identifiers_AndEstablishmentAndTrustExists_Returns_Ok()
    {
        using var context = Fixture.GetMstrContext();

        var mixedData = CreateSameUKPRNDataSet(context);

        var selectedEstablishment = mixedData.Establishments.First();
        var selectedTrust = mixedData.Trust;

        var cancellationToken = new CancellationToken();
        var response = await Sut.GetIdentifiers(MixedSameUkprn, cancellationToken);

        response.Result.Should().NotBeNull();
        response.Result.Should().BeOfType(typeof(OkObjectResult));
        response.GetStatusCode().Should().Be((int)HttpStatusCode.OK);

        var content = response.GetIdentifiers();
        content.Should().NotBeNull();
        var establishments = content!.Establishments;
        establishments.Length.Should().Be(1);
        AssertEstablishmentsIdentifierResponse(establishments.First(), selectedEstablishment);
        var trusts = content.Trusts;
        trusts.Length.Should().Be(1);
        AssertTrustIdentifierResponse(trusts.First(), selectedTrust);
    }

    private static async Task<List<Trust>> BuildTrustSet(MstrContext context)
    {
        var trusts = new List<Trust>();

        for (var idx = 0; idx < 3; idx++)
        {
            var trust = DatabaseModelBuilder.BuildTrust();
            trusts.Add(trust);
        }

        context.Trusts.AddRange(trusts);
        await context.SaveChangesAsync();
        return trusts;
    }

    private static async Task<List<Trust>> BuildDuplicateTrustSet(MstrContext context, TrustIdTypes trustId)
    {
        var trusts = new List<Trust>();

        for (var idx = 0; idx < 3; idx++)
        {
            var trust = DatabaseModelBuilder.BuildTrust();
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

    private static async Task<List<Trust>> BuildTrustSetWithEmptyData(MstrContext context,
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

    private static TrustDataSet CreateEstablishmentSet(MstrContext context)
    {
        var trust = DatabaseModelBuilder.BuildTrust();
        context.Add(trust);
        context.SaveChanges();

        var establishments = new List<Establishment>();

        for (var idx = 0; idx < 3; idx++)
        {
            var localAuthority = context.LocalAuthorities.First(la => la.SK % 3 == idx);
            var establishmentDataSet = CreateEstablishment(localAuthority);

            context.Establishments.Add(establishmentDataSet);

            establishments.Add(establishmentDataSet);
        }

        context.SaveChanges();

        var trustToEstablishmentLinks =
            LinkTrustToEstablishments(trust, establishments);

        context.EducationEstablishmentTrusts.AddRange(trustToEstablishmentLinks);

        context.SaveChanges();

        var result = new TrustDataSet(Trust: trust, Establishments: establishments);

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

    private static TrustDataSet CreateSameUKPRNDataSet(MstrContext context)
    {
        var trust = DatabaseModelBuilder.BuildTrust();
        trust.UKPRN = MixedSameUkprn;
        context.Add(trust);
        context.SaveChanges();

        //Establishment
        var establishments = new List<Establishment>();

        var establishment = CreateEstablishment(context.LocalAuthorities.First());
        establishment.UKPRN = MixedSameUkprn;

        context.Establishments.Add(establishment);

        establishments.Add(establishment);

        context.SaveChanges();

        var trustToEstablishmentLinks =
            LinkTrustToEstablishments(trust, establishments);

        context.EducationEstablishmentTrusts.AddRange(trustToEstablishmentLinks);

        context.SaveChanges();

        var result = new TrustDataSet(Trust: trust, Establishments: establishments);

        return result;
    }

    private static Establishment CreateEstablishment(LocalAuthority la)
    {
        var establishment = DatabaseModelBuilder.BuildEstablishment();

        establishment.LocalAuthority = la;

        return establishment;
    }

    private static void AssertTrustIdentifierResponse(TrustIdentifiers actual, Trust expected)
    {
        actual.TrustReference.Should().Be(expected.GroupID);
        actual.UKPRN.Should().Be(expected.UKPRN);
        actual.UID.Should().Be(expected.GroupUID);
    }

    private static void AssertEstablishmentsIdentifierResponse(EstablishmentIdentifiers actual, Establishment expected)
    {
        actual.URN.Should().Be(expected.URN.ToString());
        actual.UKPRN.Should().Be(expected.UKPRN);
        actual.LAESTAB.Should().Be($"{expected.LocalAuthority.Code}/{expected.EstablishmentNumber}");
    }

    public enum TrustIdTypes
    {
        GroupID,
        UKPRN,
        GroupUID
    }

    public enum EstablishmentsIdTypes
    {
        URN,
        UKPRN,
        LAESTAB
    }

    private record TrustDataSet(
        Trust Trust,
        List<Establishment> Establishments
    );
}