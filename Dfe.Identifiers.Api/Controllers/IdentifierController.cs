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
