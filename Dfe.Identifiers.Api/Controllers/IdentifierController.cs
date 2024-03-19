using System.Text.Json;
using System.Text.RegularExpressions;
using Dfe.Identifiers.Api.Models;
using Dfe.Identifiers.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Identifiers.Api.Controllers;

/// <summary>
/// Handles operations related to Identifiers.
/// </summary>
[ApiController]
[Route("api/")]
[SwaggerTag("Identifiers Endpoints")]
public class IdentifiersController : ControllerBase
{
    private readonly TrustRepository _trustQueries;
    private readonly EstablishmentRepository _establishmentQueries;
    private readonly ILogger<IdentifiersController> _logger;

    public IdentifiersController(TrustRepository trustQueries, ILogger<IdentifiersController> logger,
        EstablishmentRepository establishmentQueries)
    {
        _trustQueries = trustQueries;
        _logger = logger;
        _establishmentQueries = establishmentQueries;
    }

    /// <summary>
    /// Retrieves an object's other identifiers based on one of its identifiers. Currently supports UKPRN, UID and Trust Reference for trusts and UKPRN, URN and LAESTAB for establishments
    /// </summary>
    /// <param name="identifier">The identifier (UKPRN, UID, URN, LAESTAB or Trust Reference).</param>
    /// <param name="cancellationToken"></param>
    /// <returns>A list of matching objects or empty lists if not found.</returns>
    [HttpGet]
    [Route("identifier/{identifier}")]
    [SwaggerOperation(Summary = "Retrieves an object's Identifiers based on one of its identifiers.",
        Description = "Returns an objects identifiers found in the database.")]
    [SwaggerResponse(200, "Successfully found and returned the objects identifiers.")]
    public async Task<ActionResult<Identifiers>> GetIdentifiers(string identifier, CancellationToken cancellationToken)
    {
        var loggableIdentifier =
            Regex.Replace(identifier, "[^a-zA-Z0-9-]", "", RegexOptions.None, TimeSpan.FromSeconds(2));
        _logger.LogInformation("Attempting to get object identifiers by identifier {identifier}", loggableIdentifier);
        var trusts = await _trustQueries.GetTrustsByIdentifier(identifier, cancellationToken).ConfigureAwait(false);
        var establishments = await _establishmentQueries.GetEstablishmentsByIdentifier(identifier, cancellationToken)
            .ConfigureAwait(false);
        var results = new Identifiers(trusts.Select(MapToIdentifiers).ToArray(), establishments.Select(MapToIdentifiers).ToArray());
        _logger.LogInformation("Returning objects found by identifier {identifier}", loggableIdentifier);
        _logger.LogDebug("{output}", JsonSerializer.Serialize(results));
        return Ok(results);
    }

    public static EstablishmentIdentifiers MapToIdentifiers(Establishment establishment)
    {
        return new EstablishmentIdentifiers(UKPRN: establishment.UKPRN, URN: establishment.URN.ToString(),
            LAESTAB: $"{establishment.LocalAuthority.Code}/{establishment.EstablishmentNumber}");
    }

    private static TrustIdentifiers MapToIdentifiers(Trust trust)
    {
        return new TrustIdentifiers(UID: trust.GroupUID, UKPRN: trust.UKPRN, TrustReference: trust.GroupID);
    }
}

public record Identifiers(TrustIdentifiers[] Trusts, EstablishmentIdentifiers[] Establishments);

public record TrustIdentifiers(
    string? UID,
    string? UKPRN,
    string? TrustReference
);

public record EstablishmentIdentifiers(
    string? LAESTAB,
    string? UKPRN,
    string? URN
);