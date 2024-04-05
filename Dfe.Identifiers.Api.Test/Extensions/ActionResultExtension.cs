using Microsoft.AspNetCore.Mvc;

namespace Dfe.Identifiers.Api.Test.Extensions;

public static class ActionResultExtension 
{
    public static Domain.Identifiers.IdentifiersCollection? GetIdentifiers(this ActionResult<Domain.Identifiers.IdentifiersCollection> actionResult)
    {
        return (Domain.Identifiers.IdentifiersCollection?)((OkObjectResult?)actionResult.Result)?.Value;
    }
    
    public static int? GetStatusCode<T>(this ActionResult<T> actionResult)
    {
        return ((ObjectResult?)actionResult.Result)?.StatusCode;
    }
}