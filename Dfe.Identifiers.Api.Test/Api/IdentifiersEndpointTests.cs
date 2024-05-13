using System.Net;
using System.Net.Http.Json;
using Dfe.Identifiers.Api.Test.Constants;
using Dfe.Identifiers.Api.Test.Helpers;
using Dfe.Identifiers.Domain.Identifiers;
using Dfe.Identifiers.Domain.Models;
using FluentAssertions;

namespace Dfe.Identifiers.Api.Test.Api;

public class IdentifiersEndpointTests : IClassFixture<ApiTestFixture>
{
    private ApiTestFixture Fixture { get; }
    
    private readonly HttpClient _client;

    private const string ApiUrlPrefix = "/api/identifier";
    public IdentifiersEndpointTests(ApiTestFixture fixture)
    {
        Fixture = fixture;
        _client = fixture.Client;
    }

    // TRUSTS

    [Theory]
    [InlineData(TrustIdTypes.GroupID)]
    [InlineData(TrustIdTypes.UKPRN)]
    [InlineData(TrustIdTypes.GroupUID)]
    public async Task Get_TrustIdentifiers_AndTrustExists_Returns_Ok(TrustIdTypes trustIdType)
    {
        using var context = Fixture.GetMstrContext();

        var trustData = await DatabaseModelBuilder.BuildTrustSet(context);

        var selectedTrust = trustData.First();

        var identifier = trustIdType switch
        {
            TrustIdTypes.GroupID => selectedTrust.GroupID,
            TrustIdTypes.UKPRN => selectedTrust.UKPRN,
            TrustIdTypes.GroupUID => selectedTrust.GroupUID,
            _ => throw new ArgumentOutOfRangeException(nameof(trustIdType), trustIdType, null)
        };

        var response = await _client.GetAsync($"{ApiUrlPrefix}/{identifier}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
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

        var trustData = await DatabaseModelBuilder.CreateDuplicateTrustSet(context, trustIdType);

        var identifier = trustIdType switch
        {
            TrustIdTypes.GroupID => "GroupID",
            TrustIdTypes.UKPRN => "UKPRN",
            TrustIdTypes.GroupUID => "GroupUID",
            _ => throw new ArgumentOutOfRangeException(nameof(trustIdType), trustIdType, null)
        };

        var response = await _client.GetAsync($"{ApiUrlPrefix}/{identifier}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
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

        var trustData = await DatabaseModelBuilder.CreateTrustSetWithEmptyData(context, trustIdType);
        var selectedTrust = trustData.First();
        var identifier = trustIdType switch
        {
            TrustIdTypes.GroupID => selectedTrust.GroupID,
            TrustIdTypes.UKPRN => selectedTrust.UKPRN,
            TrustIdTypes.GroupUID => selectedTrust.GroupUID,
            _ => throw new ArgumentOutOfRangeException(nameof(trustIdType), trustIdType, null)
        };
        
        var response = await _client.GetAsync($"{ApiUrlPrefix}/{identifier}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
        content.Should().NotBeNull();
        var trusts = content!.Trusts;
        trusts.Length.Should().Be(1);
        AssertTrustIdentifierResponse(trusts.First(), selectedTrust);
    }

    [Fact]
    public async Task Get_TrustIdentifiers_AndTrustDoesNotExist_Returns_EmptyList()
    {
        using var context = Fixture.GetMstrContext();

        await DatabaseModelBuilder.BuildTrustSet(context);

        var response = await _client.GetAsync($"{ApiUrlPrefix}/noIdentifier");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
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

        var trustData = DatabaseModelBuilder.CreateEstablishmentSet(context);

        var selectedEstablishment = trustData.Establishments.First();
        var identifier = idType switch
        {
            EstablishmentsIdTypes.URN => $"{selectedEstablishment.URN}",
            EstablishmentsIdTypes.UKPRN => selectedEstablishment.UKPRN,
            EstablishmentsIdTypes.LAESTAB =>
                $"{selectedEstablishment.LocalAuthority.Code}%2F{selectedEstablishment.EstablishmentNumber}",
            _ => throw new ArgumentOutOfRangeException(nameof(idType), idType, null)
        };

        var response = await _client.GetAsync($"{ApiUrlPrefix}/{identifier}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
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

        var mixedData = DatabaseModelBuilder.CreateSameUKPRNDataSet(context);

        var selectedEstablishment = mixedData.Establishments.First();
        var selectedTrust = mixedData.Trust;

        var response = await _client.GetAsync($"{ApiUrlPrefix}/{IdentifierConstants.MixedSameUkprn}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
        content.Should().NotBeNull();
        var establishments = content!.Establishments;
        establishments.Length.Should().Be(1);
        AssertEstablishmentsIdentifierResponse(establishments.First(), selectedEstablishment);
        var trusts = content.Trusts;
        trusts.Length.Should().Be(1);
        AssertTrustIdentifierResponse(trusts.First(), selectedTrust);
    }

    // PROJECTS
    
    [Theory]
    [InlineData(ConversionProjectIdTypes.ApplicationReferenceNumber)]
    [InlineData(ConversionProjectIdTypes.SponsorReferenceNumber)]
    [InlineData(ConversionProjectIdTypes.TrustReferenceNumber)]
    public async Task Get_ConversionProjectIdentifier_AndProjectExistsExists_Returns_Ok(ConversionProjectIdTypes idType)
    {
        using var context = Fixture.GetAcademisationContext();

        var projectData = await DatabaseModelBuilder.CreateConversionProjectsSet(context);

        var selectedProject = projectData.First();
        var identifier = idType switch
        {
            ConversionProjectIdTypes.ApplicationReferenceNumber => selectedProject.ApplicationReferenceNumber,
            ConversionProjectIdTypes.SponsorReferenceNumber => selectedProject.SponsorReferenceNumber,
            ConversionProjectIdTypes.TrustReferenceNumber => selectedProject.TrustReferenceNumber,
            _ => throw new ArgumentOutOfRangeException(nameof(idType), idType, null)
        };

        var response = await _client.GetAsync($"{ApiUrlPrefix}/{identifier}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
        content.Should().NotBeNull();
        var conversionProjects = content!.ConversionProjects;
        conversionProjects.Length.Should().Be(1);
        AssertConversionProjectIdentifierResponse(conversionProjects.First(), selectedProject);
    }
    
    [Fact]
    public async Task Get_TransferProjectIdentifier_AndProjectExistsExists_Returns_Ok()
    {
        using var context = Fixture.GetAcademisationContext();

        var projectData = await DatabaseModelBuilder.CreateTransferProjectsSet(context);

        var selectedProject = projectData.First();
        var identifier = selectedProject.ProjectReference;

        var response = await _client.GetAsync($"{ApiUrlPrefix}/{identifier}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
        content.Should().NotBeNull();
        var transferProjects = content!.TransferProjects;
        transferProjects.Length.Should().Be(1);
        AssertTransferProjectIdentifierResponse(transferProjects.First(), selectedProject);
    }
    
    [Theory]
    [InlineData(FormAMatProjectIdTypes.ApplicationReference)]
    [InlineData(FormAMatProjectIdTypes.ReferenceNumber)]
    public async Task Get_FormAMatProjectIdentifier_AndProjectExistsExists_Returns_Ok(FormAMatProjectIdTypes idType)
    {
        using var context = Fixture.GetAcademisationContext();

        var projectData = await DatabaseModelBuilder.CreateFormAMatProjectsSet(context);

        var selectedProject = projectData.First();
        var identifier = idType switch
        {
            FormAMatProjectIdTypes.ApplicationReference => selectedProject.ApplicationReference,
            FormAMatProjectIdTypes.ReferenceNumber => selectedProject.ReferenceNumber,
            _ => throw new ArgumentOutOfRangeException(nameof(idType), idType, null)
        };

        var response = await _client.GetAsync($"{ApiUrlPrefix}/{identifier}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<IdentifiersCollection>();
        content.Should().NotBeNull();
        var formAMatProjects = content!.FormAMatProjects;
        formAMatProjects.Length.Should().Be(1);
        AssertFormAMatProjectIdentifierResponse(formAMatProjects.First(), selectedProject);
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
    
    private static void AssertConversionProjectIdentifierResponse(ConversionProjectIdentifiers actual, ConversionProject expected)
    {
        actual.ApplicationReferenceNumber.Should().Be(expected.ApplicationReferenceNumber);
        actual.SponsorReferenceNumber.Should().Be(expected.SponsorReferenceNumber);
        actual.TrustReferenceNumber.Should().Be(expected.TrustReferenceNumber);
    }
    
    private static void AssertTransferProjectIdentifierResponse(TransferProjectIdentifiers actual, TransferProject expected)
    {
        actual.ProjectReference.Should().Be(expected.ProjectReference);
    }
    
    private static void AssertFormAMatProjectIdentifierResponse(FormAMatProjectIdentifiers actual, FormAMatProject expected)
    {
        actual.ApplicationReference.Should().Be(expected.ApplicationReference);
        actual.ReferenceNumber.Should().Be(expected.ReferenceNumber);
    }
}