using System.Text.Json;
using System.Text.RegularExpressions;
using Dfe.Identifiers.Application;
using Dfe.Identifiers.Domain.Identifiers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Dfe.Identifiers.Api.Controllers;

/// <summary>
/// Handles operations related to Identifiers.
/// </summary>
[ApiController]
[Route("api/")]
[SwaggerTag("Identifiers Endpoints")]
public class IdentifiersController(ILogger<IdentifiersController> logger, IIdentifiersQuery identifiersQuery)
    : ControllerBase
{
    /// <summary>
    /// Retrieves an entity's other identifiers from a single identifier
    /// </summary>
    /// <param name="identifier">
    /// Supported identifiers:
    /// Trusts: UKPRN, UID, Trust Reference Number;
    /// Establishments: UKPRN, URN and LAESTAB;
    /// Transfer Project: Project Reference Number;
    /// Conversion Project: Application Reference Number, Trust Reference Number, Sponsor Reference Number;
    /// Form A Mat Project: Reference Number, Application Reference;
    /// (LAESTAB cannot use a slash to avoid routing errors, use a double encoded slash %252F
    /// or any other valid separtor eg hyphen)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>An object containing list of matching entities or empty lists if not found.
    /// Supported entities: Trusts, Establishments, Transfer Project, Conversion Project, Form A Mat Project
    /// </returns>
    [HttpGet]
    [Route("identifier/{identifier}")]
    [SwaggerOperation(Summary = "Retrieves an entities Identifiers based on one of its identifiers.",
        Description = "Returns an entities identifiers found in the database.")]
    [SwaggerResponse(200, "Successfully found and returned the entities identifiers.")]
    public async Task<ActionResult<IdentifiersCollection>> GetIdentifiers(string identifier, CancellationToken cancellationToken)
    {
        var loggableIdentifier =
            Regex.Replace(identifier, "[^a-zA-Z0-9-]", "", RegexOptions.None, TimeSpan.FromSeconds(2));
        logger.LogInformation("Attempting to get object identifiers by identifier {identifier}", loggableIdentifier);
        var results = await identifiersQuery.GetIdentifiers(identifier, cancellationToken);
        logger.LogInformation("Returning objects found by identifier {identifier}", loggableIdentifier);
        logger.LogDebug("{output}", JsonSerializer.Serialize(results));
        return Ok(results);
    }
   
}
